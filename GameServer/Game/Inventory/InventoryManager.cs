using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums.Item;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Util;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EggLink.DanhengServer.Game.Inventory
{
    public class InventoryManager(PlayerInstance player) : BasePlayerManager(player)
    {
        public InventoryData Data = DatabaseHelper.Instance!.GetInstanceOrCreateNew<InventoryData>(player.Uid);

        public void AddItem(ItemData itemData, bool notify = true)
        {
            PutItem(itemData.ItemId, itemData.Count, 
                itemData.Rank, itemData.Promotion, 
                itemData.Level, itemData.Exp, itemData.TotalExp, 
                itemData.MainAffix, itemData.SubAffixes,
                itemData.UniqueId);
            
            Player.SendPacket(new PacketPlayerSyncScNotify(itemData));
            if (notify)
            {
                Player.SendPacket(new PacketScenePlaneEventScNotify(itemData));
            }
            DatabaseHelper.Instance?.UpdateInstance(Data);
        }

        public void AddItems(List<ItemData> items, bool notify = true)
        {
            var syncItems = new List<ItemData>();
            foreach (var item in items)
            {
                var i = AddItem(item.ItemId, item.Count, false, sync:false, returnRaw:true);
                if (i != null)
                {
                    syncItems.Add(i);
                }
            }
            Player.SendPacket(new PacketPlayerSyncScNotify(syncItems));
            if (notify)
            {
                Player.SendPacket(new PacketScenePlaneEventScNotify(items));
            }

            DatabaseHelper.Instance?.UpdateInstance(Data);
        }

        public ItemData? AddItem(int itemId, int count, bool notify = true, int rank = 1, int level = 1, bool sync = true, bool returnRaw = false)
        {
            GameData.ItemConfigData.TryGetValue(itemId, out var itemConfig);
            if (itemConfig == null) return null;

            ItemData? itemData = null;

            switch (itemConfig.ItemMainType)
            {
                case ItemMainTypeEnum.Equipment:
                    itemData = PutItem(itemId, 1, rank: rank, level: level, uniqueId: ++Data.NextUniqueId);
                    break;
                case ItemMainTypeEnum.Usable:
                    switch (itemConfig.ItemSubType)
                    {
                        case ItemSubTypeEnum.HeadIcon:
                            Player.PlayerUnlockData!.HeadIcons.Add(itemId);
                            break;
                        case ItemSubTypeEnum.ChatBubble:
                            Player.PlayerUnlockData!.ChatBubbles.Add(itemId);
                            break;
                        case ItemSubTypeEnum.PhoneTheme:
                            Player.PlayerUnlockData!.PhoneThemes.Add(itemId);
                            break;
                        case ItemSubTypeEnum.Food:
                        case ItemSubTypeEnum.Book:
                            itemData = PutItem(itemId, count);
                            break;
                    }
                    itemData ??= new()
                    {
                        ItemId = itemId,
                        Count = count,
                    };
                    break;
                case ItemMainTypeEnum.Relic:
                    var item = PutItem(itemId, 1, rank: 1, level: 0, uniqueId: ++Data.NextUniqueId);
                    item.AddRandomRelicMainAffix();
                    item.AddRandomRelicSubAffix(3);
                    Data.RelicItems.Find(x => x.UniqueId == item.UniqueId)!.SubAffixes = item.SubAffixes;
                    itemData = item;
                    break;
                case ItemMainTypeEnum.Virtual:
                    switch (itemConfig.ID)
                    {
                        case 1:
                            Player.Data.Hcoin += count;
                            break;
                        case 2:
                            Player.Data.Scoin += count;
                            break;
                        case 3:
                            Player.Data.Mcoin += count;
                            break;
                        case 11:
                            Player.Data.Stamina += count;
                            break;
                        case 22:
                            Player.Data.Exp += count;
                            Player.OnAddExp();
                            break;
                        case 32:
                            Player.Data.TalentPoints += count;
                            // TODO : send VirtualItemPacket instead of PlayerSyncPacket
                            break;
                    }
                    if (count != 0)
                    {
                        Player.SendPacket(new PacketPlayerSyncScNotify(Player.ToProto()));
                        itemData = new()
                        {
                            ItemId = itemId,
                            Count = count,
                        };
                    }
                    break;
                case ItemMainTypeEnum.AvatarCard:
                    // add avatar
                    var avatar = Player.AvatarManager?.GetAvatar(itemId);
                    if (avatar != null && avatar.Excel != null)
                    {
                        var rankUpItem = Player.InventoryManager!.GetItem(avatar.Excel.RankUpItemId);
                        if ((avatar.Rank + rankUpItem?.Count ?? 0) <= 5)
                            itemData = PutItem(avatar.Excel.RankUpItemId, 1);
                    }
                    else
                    {
                        Player.AddAvatar(itemId, sync, notify);
                        AddItem(itemId + 200000, 1, false);
                    }
                    break;
                default:
                    itemData = PutItem(itemId, Math.Min(count, itemConfig.PileLimit));
                    break;
            }

            ItemData? clone = null;
            if (itemData != null)
            {
                clone = itemData.Clone();
                if (sync)
                    Player.SendPacket(new PacketPlayerSyncScNotify(itemData));
                clone.Count = count;
                if (notify)
                {
                    Player.SendPacket(new PacketScenePlaneEventScNotify(clone));
                }
            }

            return returnRaw ? itemData : clone ?? itemData;
        }

        public ItemData PutItem(int itemId, int count, int rank = 0, int promotion = 0, int level = 0, int exp = 0, int totalExp = 0, int mainAffix = 0, List<ItemSubAffix>? subAffixes = null, int uniqueId = 0)
        {
            if (promotion == 0 && level > 10)
            {
                promotion = GameData.GetMinPromotionForLevel(level);
            }
            var item = new ItemData()
            {
                ItemId = itemId,
                Count = count,
                Rank = rank,
                Promotion = promotion,
                Level = level,
                Exp = exp,
                TotalExp = totalExp,
                MainAffix = mainAffix,
                SubAffixes = subAffixes ?? [],
            };

            if (uniqueId > 0)
            {
                item.UniqueId = uniqueId;
            }

            switch (GameData.ItemConfigData[itemId].ItemMainType)
            {
                case ItemMainTypeEnum.Material:
                case ItemMainTypeEnum.Virtual:
                case ItemMainTypeEnum.Usable:
                case ItemMainTypeEnum.Mission:
                    var oldItem = Data.MaterialItems.Find(x => x.ItemId == itemId);
                    if (oldItem != null)
                    {
                        oldItem.Count += count;
                        item = oldItem;
                        break;
                    }
                    Data.MaterialItems.Add(item);
                    break;
                case ItemMainTypeEnum.Equipment:
                    Data.EquipmentItems.Add(item);
                    break;
                case ItemMainTypeEnum.Relic:
                    Data.RelicItems.Add(item);
                    break;
            }

            return item;
        }

        public List<ItemData> RemoveItems(List<(int itemId, int count, int uniqueId)> items, bool sync = true)
        {
            List<ItemData> removedItems = new List<ItemData>();
            foreach (var item in items)
            {
                var removedItem = RemoveItem(item.itemId, item.count, item.uniqueId, sync: false);
                if (removedItem != null)
                {
                    removedItems.Add(removedItem);
                }
            }
            if (sync && removedItems.Count > 0)
            {
                Player.SendPacket(new PacketPlayerSyncScNotify(removedItems));
            }
            DatabaseHelper.Instance?.UpdateInstance(Data);
            return removedItems;
        }

        public ItemData? RemoveItem(int itemId, int count, int uniqueId = 0, bool sync = true)
        {
            GameData.ItemConfigData.TryGetValue(itemId, out var itemConfig);
            if (itemConfig == null)
            {
                return null;
            }

            ItemData? itemData = null;

            switch (itemConfig.ItemMainType)
            {
                case ItemMainTypeEnum.Material:
                case ItemMainTypeEnum.Mission:
                    var item = Data.MaterialItems.Find(x => x.ItemId == itemId);
                    if (item == null)
                    {
                        return null;
                    }
                    item.Count -= count;
                    if (item.Count <= 0)
                    {
                        Data.MaterialItems.Remove(item);
                        item.Count = 0;
                    }
                    itemData = item;
                    break;
                case ItemMainTypeEnum.Virtual:
                    switch (itemConfig.ID)
                    {
                        case 1:
                            Player.Data.Hcoin -= count;
                            itemData = new ItemData { ItemId = itemId, Count = count };
                            break;
                        case 2:
                            Player.Data.Scoin -= count;
                            itemData = new ItemData { ItemId = itemId, Count = count };
                            break;
                        case 3:
                            Player.Data.Mcoin -= count;
                            itemData = new ItemData { ItemId = itemId, Count = count };
                            break;
                        case 32:
                            Player.Data.TalentPoints -= count;
                            itemData = new ItemData { ItemId = itemId, Count = count };
                            break;
                    }
                    if (sync && itemData != null)
                    {
                        Player.SendPacket(new PacketPlayerSyncScNotify(itemData));
                    }
                    break;
                case ItemMainTypeEnum.Equipment:
                    var equipment = Data.EquipmentItems.Find(x => x.UniqueId == uniqueId);
                    if (equipment == null)
                    {
                        return null;
                    }
                    Data.EquipmentItems.Remove(equipment);
                    equipment.Count = 0;
                    itemData = equipment;
                    break;
                case ItemMainTypeEnum.Relic:
                    var relic = Data.RelicItems.Find(x => x.UniqueId == uniqueId);
                    if (relic == null)
                    {
                        return null;
                    }
                    Data.RelicItems.Remove(relic);
                    relic.Count = 0;
                    itemData = relic;
                    break;
            }
            if (itemData != null && sync)
            {
                Player.SendPacket(new PacketPlayerSyncScNotify(itemData));
            }
            DatabaseHelper.Instance?.UpdateInstance(Data);
            return itemData;
        }

        public ItemData? GetItem(int itemId)
        {
            GameData.ItemConfigData.TryGetValue(itemId, out var itemConfig);
            if (itemConfig == null) return null;
            switch (itemConfig.ItemMainType)
            {
                case ItemMainTypeEnum.Material:
                    return Data.MaterialItems.Find(x => x.ItemId == itemId);
                case ItemMainTypeEnum.Equipment:
                    return Data.EquipmentItems.Find(x => x.ItemId == itemId);
                case ItemMainTypeEnum.Relic:
                    return Data.RelicItems.Find(x => x.ItemId == itemId);
                case ItemMainTypeEnum.Virtual:
                    switch (itemConfig.ID)
                    {
                        case 1:
                            return new ItemData()
                            {
                                ItemId = itemId,
                                Count = Player.Data.Hcoin,
                            };
                        case 2:
                            return new ItemData()
                            {
                                ItemId = itemId,
                                Count = Player.Data.Scoin,
                            };
                        case 3:
                            return new ItemData()
                            {
                                ItemId = itemId,
                                Count = Player.Data.Mcoin,
                            };
                        case 11:
                            return new ItemData()
                            {
                                ItemId = itemId,
                                Count = Player.Data.Stamina,
                            };
                        case 22:
                            return new ItemData()
                            {
                                ItemId = itemId,
                                Count = Player.Data.Exp,
                            };
                        case 32:
                            return new ItemData()
                            {
                                ItemId = itemId,
                                Count = Player.Data.TalentPoints,
                            };
                    }
                    break;
            }
            return null;
        }

        public void HandlePlaneEvent(int eventId)
        {
            GameData.PlaneEventData.TryGetValue(eventId * 10 + Player.Data.WorldLevel, out var planeEvent);
            if (planeEvent == null) return;
            GameData.RewardDataData.TryGetValue(planeEvent.Reward, out var rewardData);
            rewardData?.GetItems().ForEach(x => AddItem(x.Item1, x.Item2));

            foreach (var id in planeEvent.DropList)
            {
                GameData.RewardDataData.TryGetValue(id, out var reward);
                reward?.GetItems().ForEach(x => AddItem(x.Item1, x.Item2));
            }
        }

        public List<ItemData> HandleMappingInfo(int mappingId, int worldLevel)
        {
            // calculate drops
            List<ItemData> items = [];
            GameData.MappingInfoData.TryGetValue(mappingId * 10 + worldLevel, out var mapping);
            if (mapping != null)
            {
                foreach (var item in mapping.DropItemList)
                {
                    var random = Random.Shared.Next(0, 101);

                    if (random <= item.Chance)
                    {
                        var amount = item.ItemNum > 0 ? item.ItemNum : Random.Shared.Next(item.MinCount, item.MaxCount + 1);

                        GameData.ItemConfigData.TryGetValue(item.ItemID, out var itemData);
                        if (itemData == null) continue;

                        items.Add(new ItemData()
                        {
                            ItemId = item.ItemID,
                            Count = amount,
                        });
                    }
                }

                // randomize the order of the relics
                var relics = mapping.DropRelicItemList.OrderBy(x => Random.Shared.Next()).ToList();

                var relic5Count = Random.Shared.Next(worldLevel - 4, worldLevel - 2);
                var relic4Count = worldLevel - 2;
                foreach (var relic in relics)
                {
                    var random = Random.Shared.Next(0, 101);

                    if (random <= relic.Chance)
                    {
                        var amount = relic.ItemNum > 0 ? relic.ItemNum : Random.Shared.Next(relic.MinCount, relic.MaxCount + 1);

                        GameData.ItemConfigData.TryGetValue(relic.ItemID, out var itemData);
                        if (itemData == null) continue;

                        if (itemData.Rarity == ItemRarityEnum.SuperRare && relic5Count > 0)
                        {
                            relic5Count--;
                        }
                        else if (itemData.Rarity == ItemRarityEnum.VeryRare && relic4Count > 0)
                        {
                            relic4Count--;
                        }
                        else
                        {
                            continue;
                        }

                        items.Add(new ItemData()
                        {
                            ItemId = relic.ItemID,
                            Count = 1,
                        });
                    }
                }

                foreach (var item in items)
                {
                    var i = Player.InventoryManager!.AddItem(item.ItemId, item.Count, false)!;
                    i.Count = item.Count;  // return the all thing
                }

                DatabaseHelper.Instance!.UpdateInstance(Player.InventoryManager!.Data);
            }

            return items;
        }

        public ItemData? ComposeItem(int composeId, int count)
        {
            GameData.ItemComposeConfigData.TryGetValue(composeId, out var composeConfig);
            if (composeConfig == null) return null;
            foreach (var cost in composeConfig.MaterialCost)
            {
                RemoveItem(cost.ItemID, cost.ItemNum * count);
            }

            RemoveItem(2, composeConfig.CoinCost * count);

            return AddItem(composeConfig.ItemID, count, false);
        }

        public List<ItemData> SellItem(ItemCostData costData)
        {
            List<ItemData> items = new List<ItemData>();
            Dictionary<int, int> itemMap = new Dictionary<int, int>();
            List<(int itemId, int count, int uniqueId)> removeItems = new List<(int itemId, int count, int uniqueId)>();

            foreach (var cost in costData.ItemList)
            {
                if (cost.EquipmentUniqueId != 0)  // equipment
                {
                    var itemData = Data.EquipmentItems.Find(x => x.UniqueId == cost.EquipmentUniqueId);
                    if (itemData == null) continue;
                    removeItems.Add((itemData.ItemId, 1, (int)cost.EquipmentUniqueId));
                    GameData.ItemConfigData.TryGetValue(itemData.ItemId, out var itemConfig);
                    if (itemConfig == null) continue;
                    foreach (var returnItem in itemConfig.ReturnItemIDList)  // return items
                    {
                        if (!itemMap.ContainsKey(returnItem.ItemID))
                        {
                            itemMap[returnItem.ItemID] = 0;
                        }
                        itemMap[returnItem.ItemID] += returnItem.ItemNum;
                    }
                }
                else if (cost.RelicUniqueId != 0)  // relic
                {
                    var itemData = Data.RelicItems.Find(x => x.UniqueId == cost.RelicUniqueId);
                    if (itemData == null) continue;
                    removeItems.Add((itemData.ItemId, 1, (int)cost.RelicUniqueId));
                    GameData.ItemConfigData.TryGetValue(itemData.ItemId, out var itemConfig);
                    if (itemConfig == null) continue;
                    foreach (var returnItem in itemConfig.ReturnItemIDList)  // return items
                    {
                        if (!itemMap.ContainsKey(returnItem.ItemID))
                        {
                            itemMap[returnItem.ItemID] = 0;
                        }
                        itemMap[returnItem.ItemID] += returnItem.ItemNum;
                    }
                }
                else
                {
                    removeItems.Add(((int)cost.PileItem.ItemId, (int)cost.PileItem.ItemNum, 0));
                }
            }

            var removedItems = RemoveItems(removeItems);

            foreach (var itemInfo in itemMap)
            {
                var item = AddItem(itemInfo.Key, itemInfo.Value, false);
                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        #region Equip

        public void EquipAvatar(int baseAvatarId, int equipmentUniqueId)
        {
            var itemData = Data.EquipmentItems.Find(x => x.UniqueId == equipmentUniqueId);
            var avatarData = Player.AvatarManager!.GetAvatar(baseAvatarId);
            if (itemData == null || avatarData == null) return;
            var oldItem = Data.EquipmentItems.Find(x => x.UniqueId == avatarData.EquipId);
            if (itemData.EquipAvatar > 0)  // already be dressed
            {
                var equipAvatar = Player.AvatarManager.GetAvatar(itemData.EquipAvatar);
                if (equipAvatar != null && oldItem != null)
                {
                    // switch
                    equipAvatar.EquipId = oldItem.UniqueId;
                    oldItem.EquipAvatar = equipAvatar.GetAvatarId();
                    Player.SendPacket(new PacketPlayerSyncScNotify(equipAvatar, oldItem));
                }
            } else
            {
                if (oldItem != null)
                {
                    oldItem.EquipAvatar = 0;
                    Player.SendPacket(new PacketPlayerSyncScNotify(oldItem));
                }
            }
            itemData.EquipAvatar = avatarData.GetAvatarId();
            avatarData.EquipId = itemData.UniqueId;
            Player.SendPacket(new PacketPlayerSyncScNotify(avatarData, itemData));
        }

        public void EquipRelic(int baseAvatarId, int relicUniqueId, int slot)
        {
            var itemData = Data.RelicItems.Find(x => x.UniqueId == relicUniqueId);
            var avatarData = Player.AvatarManager!.GetAvatar(baseAvatarId);
            if (itemData == null || avatarData == null) return;
            avatarData.Relic.TryGetValue(slot, out int id);
            var oldItem = Data.RelicItems.Find(x => x.UniqueId == id);

            if (itemData.EquipAvatar > 0)  // already be dressed
            {
                var equipAvatar = Player.AvatarManager!.GetAvatar(itemData.EquipAvatar);
                if (equipAvatar != null && oldItem != null)
                {
                    // switch
                    equipAvatar.Relic[slot] = oldItem.UniqueId;
                    oldItem.EquipAvatar = equipAvatar.GetAvatarId();
                    Player.SendPacket(new PacketPlayerSyncScNotify(equipAvatar, oldItem));
                }
            } else
            {
                if (oldItem != null)
                {
                    oldItem.EquipAvatar = 0;
                    Player.SendPacket(new PacketPlayerSyncScNotify(oldItem));
                }
            }
            itemData.EquipAvatar = avatarData.GetAvatarId();
            avatarData.Relic[slot] = itemData.UniqueId;
            // save
            DatabaseHelper.Instance!.UpdateInstance(Data);
            DatabaseHelper.Instance!.UpdateInstance(Player.AvatarManager.AvatarData!);
            Player.SendPacket(new PacketPlayerSyncScNotify(avatarData, itemData));
        }

        public void UnequipRelic(int baseAvatarId, int slot)
        {
            var avatarData = Player.AvatarManager!.GetAvatar(baseAvatarId);
            if (avatarData == null) return;
            avatarData.Relic.TryGetValue(slot, out var uniqueId);
            var itemData = Data.RelicItems.Find(x => x.UniqueId == uniqueId);
            if (itemData == null) return;
            avatarData.Relic.Remove(slot);
            itemData.EquipAvatar = 0;
            Player.SendPacket(new PacketPlayerSyncScNotify(avatarData, itemData));
        }

        public void UnequipEquipment(int baseAvatarId)
        {
            var avatarData = Player.AvatarManager!.GetAvatar(baseAvatarId);
            if (avatarData == null) return;
            var itemData = Data.EquipmentItems.Find(x => x.UniqueId == avatarData.EquipId);
            if (itemData == null) return;
            itemData.EquipAvatar = 0;
            avatarData.EquipId = 0;
            Player.SendPacket(new PacketPlayerSyncScNotify(avatarData, itemData));
        }

        public List<ItemData> LevelUpAvatar(int baseAvatarId, ItemCostData item)
        {
            var avatarData = Player.AvatarManager!.GetAvatar(baseAvatarId);
            if (avatarData == null) return [];
            GameData.AvatarPromotionConfigData.TryGetValue(avatarData.AvatarId * 10 + avatarData.Promotion, out var promotionConfig);
            if (promotionConfig == null) return [];
            var exp = 0;

            foreach (var cost in item.ItemList)
            {
                GameData.ItemConfigData.TryGetValue((int)cost.PileItem.ItemId, out var itemConfig);
                if (itemConfig == null) continue;
                exp += itemConfig.Exp * (int)cost.PileItem.ItemNum;
            }

            // payment
            int costScoin = exp / 10;
            if (Player.Data.Scoin < costScoin) return [];
            foreach (var cost in item.ItemList)
            {
                RemoveItem((int)cost.PileItem.ItemId, (int)cost.PileItem.ItemNum);
            }
            RemoveItem(2, costScoin);

            int maxLevel = promotionConfig.MaxLevel;
            int curExp = avatarData.Exp;
            int curLevel = avatarData.Level;
            int nextLevelExp = GameData.GetAvatarExpRequired(avatarData.Excel!.ExpGroup, avatarData.Level);
            do
            {
                int toGain;
                if (curExp + exp >= nextLevelExp)
                {
                    toGain = nextLevelExp - curExp;
                } else
                {
                    toGain = exp;
                }
                curExp += toGain;
                exp -= toGain;
                // level up
                if (curExp >= nextLevelExp)
                {
                    curExp = 0;
                    curLevel++;
                    nextLevelExp = GameData.GetAvatarExpRequired(avatarData.Excel!.ExpGroup, curLevel);
                }
            } while (exp > 0 && nextLevelExp > 0 && curLevel < maxLevel);

            avatarData.Level = curLevel;
            avatarData.Exp = curExp;
            DatabaseHelper.Instance!.UpdateInstance(Player.AvatarManager.AvatarData!);
            // leftover
            List<ItemData> list = [];
            var leftover = exp;
            while (leftover > 0)
            {
                var gain = false;
                foreach (var expItem in GameData.AvatarExpItemConfigData.Values.Reverse())
                {
                    if (leftover >= expItem.Exp)
                    {
                        // add
                        list.Add(PutItem(expItem.ItemID, 1));
                        leftover -= expItem.Exp;
                        gain = true;
                        break;
                    }
                }
                if (!gain)
                {
                    break;  // no more item
                }
            }
            if (list.Count > 0)
            {
                Player.SendPacket(new PacketPlayerSyncScNotify(list));
            }
            Player.SendPacket(new PacketPlayerSyncScNotify(avatarData));

            return list;
        }

        #endregion

        #region Levelup

        public List<ItemData> LevelUpEquipment(int equipmentUniqueId, ItemCostData item)
        {
            var itemData = Data.EquipmentItems.Find(x => x.UniqueId == equipmentUniqueId);
            if (itemData == null) return [];
            GameData.EquipmentPromotionConfigData.TryGetValue(itemData.ItemId * 10 + itemData.Promotion, out var equipmentPromotionConfig);
            GameData.EquipmentConfigData.TryGetValue(itemData.ItemId, out var equipmentConfig);
            if (equipmentConfig == null || equipmentPromotionConfig == null) return [];
            var exp = 0;

            foreach (var cost in item.ItemList)
            {
                if (cost.PileItem == null)
                {
                    // TODO : add equipment
                    exp += 100;
                } else
                {
                    GameData.ItemConfigData.TryGetValue((int)cost.PileItem.ItemId, out var itemConfig);
                    if (itemConfig == null) continue;
                    exp += itemConfig.Exp * (int)cost.PileItem.ItemNum;
                }
            }

            // payment
            int costScoin = exp / 2;
            if (Player.Data.Scoin < costScoin) return [];
            foreach (var cost in item.ItemList)
            {
                if (cost.PileItem == null)
                {
                    // TODO : add equipment
                    var costItem = Data.EquipmentItems.Find(x => x.UniqueId == cost.EquipmentUniqueId);
                    if (costItem == null) continue;
                    RemoveItem(costItem.ItemId, 1, (int)cost.EquipmentUniqueId);
                } else
                {
                    RemoveItem((int)cost.PileItem.ItemId, (int)cost.PileItem.ItemNum);
                }
            }
            RemoveItem(2, costScoin);

            int maxLevel = equipmentPromotionConfig.MaxLevel;
            int curExp = itemData.Exp;
            int curLevel = itemData.Level;
            int nextLevelExp = GameData.GetEquipmentExpRequired(equipmentConfig.ExpType, itemData.Level);
            do
            {
                int toGain;
                if (curExp + exp >= nextLevelExp)
                {
                    toGain = nextLevelExp - curExp;
                } else
                {
                    toGain = exp;
                }
                curExp += toGain;
                exp -= toGain;
                // level up
                if (curExp >= nextLevelExp)
                {
                    curExp = 0;
                    curLevel++;
                    nextLevelExp = GameData.GetEquipmentExpRequired(equipmentConfig.ExpType, curLevel);
                }
            } while (exp > 0 && nextLevelExp > 0 && curLevel < maxLevel);

            itemData.Level = curLevel;
            itemData.Exp = curExp;
            DatabaseHelper.Instance!.UpdateInstance(Data);
            // leftover
            List<ItemData> list = [];
            var leftover = exp;
            while (leftover > 0)
            {
                var gain = false;
                foreach (var expItem in GameData.EquipmentExpItemConfigData.Values.Reverse())
                {
                    if (leftover >= expItem.ExpProvide)
                    {
                        // add
                        list.Add(PutItem(expItem.ItemID, 1));
                        leftover -= expItem.ExpProvide;
                        gain = true;
                        break;
                    }
                }
                if (!gain)
                {
                    break;  // no more item
                }
            }
            if (list.Count > 0)
            {
                Player.SendPacket(new PacketPlayerSyncScNotify(list));
            }
            Player.SendPacket(new PacketPlayerSyncScNotify(itemData));
            return list;
        }

        public Boolean promoteAvatar(int avatarId) {
            // Get avatar
            AvatarInfo avatarData = Player.AvatarManager!.GetAvatar(avatarId)!;
            if (avatarData == null || avatarData.Excel == null || avatarData.Promotion >= avatarData.Excel.MaxPromotion) return false;
            
            // Get promotion data
            Data.Excel.AvatarPromotionConfigExcel promotion = GameData.AvatarPromotionConfigData.Values.FirstOrDefault(x => x.AvatarID == avatarId && x.Promotion == avatarData.Promotion)!;

            // Sanity check
            if ((promotion == null) || avatarData.Level < promotion.MaxLevel || Player.Data.Level < promotion.PlayerLevelRequire || Player.Data.WorldLevel < promotion.WorldLevelRequire) {
                return false;
            }

            // Pay items
            foreach (var cost in promotion.PromotionCostList) {
                Player.InventoryManager!.RemoveItem(cost.ItemID, cost.ItemNum);
            }

            // Promote
            avatarData.Promotion = avatarData.Promotion + 1;

            // Send packets
            Player.SendPacket(new PacketPlayerSyncScNotify(avatarData));
            return true;
        }
        public bool PromoteEquipment(int equipmentUniqueId)
        {
            var equipmentData = Player.InventoryManager!.Data.EquipmentItems.FirstOrDefault(x => x.UniqueId == equipmentUniqueId);
            if (equipmentData == null || equipmentData.Promotion >= GameData.EquipmentConfigData[equipmentData.ItemId].MaxPromotion) return false;

            var promotionConfig = GameData.EquipmentPromotionConfigData.Values
                .FirstOrDefault(x => x.EquipmentID == equipmentData.ItemId && x.Promotion == equipmentData.Promotion);

            if (promotionConfig == null || equipmentData.Level < promotionConfig.MaxLevel || Player.Data.WorldLevel < promotionConfig.WorldLevelRequire)
            {
                return false;
            }

            foreach (var cost in promotionConfig.PromotionCostList)
            {
                Player.InventoryManager!.RemoveItem(cost.ItemID, cost.ItemNum);
            }

            equipmentData.Promotion++;
            DatabaseHelper.Instance!.UpdateInstance(Player.InventoryManager.Data);
            Player.SendPacket(new PacketPlayerSyncScNotify(equipmentData));

            return true;
        }
        public List<ItemData> LevelUpRelic(int uniqueId, ItemCostData costData)
        {
            var relicItem = Data.RelicItems.Find(x => x.UniqueId == uniqueId);
            if (relicItem == null) return [];

            var exp = 0;
            var money = 0;
            foreach (var cost in costData.ItemList)
            {
                if (cost.PileItem != null)
                {
                    GameData.RelicExpItemData.TryGetValue((int)cost.PileItem.ItemId, out var excel);
                    if (excel != null)
                    {
                        exp += excel.ExpProvide * (int)cost.PileItem.ItemNum;
                        money += excel.CoinCost * (int)cost.PileItem.ItemNum;
                    }

                    RemoveItem((int)cost.PileItem.ItemId, (int)cost.PileItem.ItemNum);
                } else if (cost.RelicUniqueId != 0)
                {
                    var costItem = Data.RelicItems.Find(x => x.UniqueId == cost.RelicUniqueId);
                    if (costItem != null)
                    {
                        GameData.RelicConfigData.TryGetValue(costItem.ItemId, out var costExcel);
                        if (costExcel == null) continue;

                        if (costItem.Level > 0) 
                        {
                            foreach (var level in Enumerable.Range(0, costItem.Level))
                            {
                                GameData.RelicExpTypeData.TryGetValue(costExcel.ExpType * 100 + level, out var typeExcel);
                                if (typeExcel != null)
                                    exp += typeExcel.Exp;
                            }
                        } else
                        {
                            exp += costExcel.ExpProvide;
                        }
                        exp += costItem.Exp;
                        money += costExcel.CoinCost;

                        RemoveItem(costItem.ItemId, 1, (int)cost.RelicUniqueId);
                    }
                }
            }

            // credit
            RemoveItem(2, money);

            // level up
            GameData.RelicConfigData.TryGetValue(relicItem.ItemId, out var relicExcel);
            if (relicExcel == null) return [];

            GameData.RelicExpTypeData.TryGetValue(relicExcel.ExpType * 100 + relicItem.Level, out var relicType);
            do
            {
                if (relicType == null) break;
                int toGain;
                if (relicItem.Exp + exp >= relicType.Exp)
                {
                    toGain = relicType.Exp - relicItem.Exp;
                }
                else
                {
                    toGain = exp;
                }
                relicItem.Exp += toGain;
                exp -= toGain;

                // level up
                if (relicItem.Exp >= relicType.Exp)
                {
                    relicItem.Exp = 0;
                    relicItem.Level++;
                    GameData.RelicExpTypeData.TryGetValue(relicExcel.ExpType * 100 + relicItem.Level, out relicType);
                    // relic attribute
                    if (relicItem.Level % 3 == 0)
                    {
                        if (relicItem.SubAffixes.Count >= 4)
                        {
                            relicItem.IncreaseRandomRelicSubAffix();
                        }
                        else
                        {
                            relicItem.AddRandomRelicSubAffix();
                        }
                    }
                }
            } while (exp > 0 && relicType?.Exp > 0 && relicItem.Level < relicExcel.MaxLevel);

            // leftover
            Dictionary<int, ItemData> list = [];
            var leftover = exp;
            while (leftover > 0)
            {
                var gain = false;
                foreach (var expItem in GameData.RelicExpItemData.Values.Reverse())
                {
                    if (leftover >= expItem.ExpProvide)
                    {
                        // add
                        PutItem(expItem.ItemID, 1);
                        if (list.TryGetValue(expItem.ItemID, out var i))
                        {
                            i.Count++;
                        } else
                        {
                            i = new ItemData()
                            {
                                ItemId = expItem.ItemID,
                                Count = 1
                            };
                            list[expItem.ItemID] = i;
                        }
                        leftover -= expItem.ExpProvide;
                        gain = true;
                        break;
                    }
                }
                if (!gain)
                {
                    break;  // no more item
                }
            }
            if (list.Count > 0)
            {
                Player.SendPacket(new PacketPlayerSyncScNotify(list.Values.ToList()));
            }
            DatabaseHelper.Instance!.UpdateInstance(Data);

            // sync
            Player.SendPacket(new PacketPlayerSyncScNotify(relicItem));

            return [.. list.Values];
        }

        public void RankUpAvatar(int baseAvatarId, ItemCostData costData)
        {
            foreach (var cost in costData.ItemList)
            {
                RemoveItem((int)cost.PileItem.ItemId, (int)cost.PileItem.ItemNum);
            }
            var avatarData = Player.AvatarManager!.GetAvatar(baseAvatarId);
            if (avatarData == null) return;
            avatarData.Rank++;
            DatabaseHelper.Instance!.UpdateInstance(Player.AvatarManager.AvatarData!);
            Player.SendPacket(new PacketPlayerSyncScNotify(avatarData));
        }

        public void RankUpEquipment(int equipmentUniqueId, ItemCostData costData)
        {
            var rank = 0;
            foreach (var cost in costData.ItemList)
            {
                var costItem = Data.EquipmentItems.Find(x => x.UniqueId == cost.EquipmentUniqueId);
                if (costItem == null) continue;
                RemoveItem(costItem.ItemId, 0, (int)cost.EquipmentUniqueId);
                rank++;
            }
            var itemData = Data.EquipmentItems.Find(x => x.UniqueId == equipmentUniqueId);
            if (itemData == null) return;
            itemData.Rank += rank;
            DatabaseHelper.Instance!.UpdateInstance(Data);
            Player.SendPacket(new PacketPlayerSyncScNotify(itemData));
        }

        #endregion
    }
}
