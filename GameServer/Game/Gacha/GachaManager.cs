using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Gacha;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Enums.Item;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Proto;
using GachaInfo = EggLink.DanhengServer.Database.Gacha.GachaInfo;

namespace EggLink.DanhengServer.GameServer.Game.Gacha;

public class GachaManager(PlayerInstance player) : BasePlayerManager(player)
{
    public GachaData GachaData { get; } = DatabaseHelper.Instance!.GetInstanceOrCreateNew<GachaData>(player.Uid);

    public List<int> GetPurpleAvatars()
    {
        var purpleAvatars = new List<int>();
        foreach (var avatar in GameData.AvatarConfigData.Values)
            if (avatar.Rarity == RarityEnum.CombatPowerAvatarRarityType4 &&
                !(GameData.MultiplePathAvatarConfigData.ContainsKey(avatar.AvatarID) &&
                  GameData.MultiplePathAvatarConfigData[avatar.AvatarID].BaseAvatarID != avatar.AvatarID))
                purpleAvatars.Add(avatar.AvatarID);
        return purpleAvatars;
    }

    public List<int> GetGoldAvatars()
    {
        return [1003, 1004, 1101, 1107, 1104, 1209, 1211];
    }

    public List<int> GetAllGoldAvatars()
    {
        var avatars = new List<int>();
        foreach (var avatar in GameData.AvatarConfigData.Values)
            if (avatar.Rarity == RarityEnum.CombatPowerAvatarRarityType5)
                avatars.Add(avatar.AvatarID);
        return avatars;
    }

    public List<int> GetBlueWeapons()
    {
        var purpleWeapons = new List<int>();
        foreach (var weapon in GameData.EquipmentConfigData.Values)
            if (weapon.Rarity == RarityEnum.CombatPowerLightconeRarity3)
                purpleWeapons.Add(weapon.EquipmentID);
        return purpleWeapons;
    }

    public List<int> GetPurpleWeapons()
    {
        var purpleWeapons = new List<int>();
        foreach (var weapon in GameData.EquipmentConfigData.Values)
            if (weapon.Rarity == RarityEnum.CombatPowerLightconeRarity4)
                purpleWeapons.Add(weapon.EquipmentID);
        return purpleWeapons;
    }

    public List<int> GetGoldWeapons()
    {
        return [23000, 23002, 23003, 23004, 23005, 23012, 23013];
    }

    public List<int> GetAllGoldWeapons()
    {
        var weapons = new List<int>();
        foreach (var weapon in GameData.EquipmentConfigData.Values)
            if (weapon.Rarity == RarityEnum.CombatPowerLightconeRarity5)
                weapons.Add(weapon.EquipmentID);
        return weapons;
    }

    public int GetRarity(int itemId)
    {
        if (GetAllGoldAvatars().Contains(itemId) || GetAllGoldWeapons().Contains(itemId)) return 5;

        if (GetPurpleAvatars().Contains(itemId) || GetPurpleWeapons().Contains(itemId)) return 4;

        if (GetBlueWeapons().Contains(itemId)) return 3;

        return 0;
    }

    public int GetType(int itemId)
    {
        if (GetAllGoldAvatars().Contains(itemId) || GetPurpleAvatars().Contains(itemId)) return 1;

        if (GetAllGoldWeapons().Contains(itemId) || GetPurpleWeapons().Contains(itemId) ||
            GetBlueWeapons().Contains(itemId)) return 2;

        return 0;
    }

    public async ValueTask<DoGachaScRsp?> DoGacha(int bannerId, int times)
    {
        var banner = GameData.BannersConfig.Banners.Find(x => x.GachaId == bannerId);
        if (banner == null) return null;
        Player.InventoryManager?.RemoveItem(banner.GachaType.GetCostItemId(), times);

        var items = new List<int>();
        for (var i = 0; i < times; i++)
        {
            var item = banner.DoGacha(GetGoldAvatars(), GetPurpleAvatars(), GetPurpleWeapons(), GetGoldWeapons(),
                GetBlueWeapons(), GachaData);
            items.Add(item);
        }

        var gachaItems = new List<GachaItem>();
        var syncItems = new List<ItemData>();
        // get rarity of item
        foreach (var item in items)
        {
            var dirt = 0;
            var star = 0;
            var rarity = GetRarity(item);

            GachaData.GachaHistory.Add(new GachaInfo
            {
                GachaId = bannerId,
                ItemId = item,
                Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            });
            var gachaItem = new GachaItem();
            if (rarity == 5)
            {
                var type = GetType(item);
                if (type == 1)
                {
                    var avatar = Player.AvatarManager?.GetAvatar(item);
                    if (avatar != null)
                    {
                        star += 40;
                        var rankUpItemId = avatar.Excel?.RankUpItemId;
                        if (rankUpItemId != null)
                        {
                            var rankUpItem = Player.InventoryManager!.GetItem(rankUpItemId.Value);
                            if (avatar.PathInfoes[item].Rank + rankUpItem?.Count >= 6)
                            {
                                star += 60;
                            }
                            else
                            {
                                var dupeItem = new ItemList();
                                dupeItem.ItemList_.Add(new Item
                                {
                                    ItemId = (uint)rankUpItemId.Value,
                                    Num = 1
                                });
                                gachaItem.TransferItemList = dupeItem;
                            }
                        }
                    }
                }
                else
                {
                    star += 20;
                }
            }
            else if (rarity == 4)
            {
                var type = GetType(item);
                if (type == 1)
                {
                    var avatar = Player.AvatarManager?.GetAvatar(item);
                    if (avatar != null)
                    {
                        star += 8;
                        var rankUpItemId = avatar.Excel?.RankUpItemId;
                        if (rankUpItemId != null)
                        {
                            var rankUpItem = Player.InventoryManager!.GetItem(rankUpItemId.Value);
                            if (avatar.PathInfoes[item].Rank + rankUpItem?.Count >= 6)
                            {
                                star += 12;
                            }
                            else
                            {
                                var dupeItem = new ItemList();
                                dupeItem.ItemList_.Add(new Item
                                {
                                    ItemId = (uint)rankUpItemId.Value,
                                    Num = 1
                                });
                                gachaItem.TransferItemList = dupeItem;
                            }
                        }
                    }
                }
                else
                {
                    star += 8;
                }
            }
            else
            {
                dirt += 20;
            }

            ItemData? i;
            if (GameData.ItemConfigData[item].ItemMainType == ItemMainTypeEnum.AvatarCard &&
                Player.AvatarManager!.GetAvatar(item) == null)
            {
                i = null;
                await Player.AvatarManager!.AddAvatar(item, isGacha: true);
            }

            else
            {
                i = await Player.InventoryManager!.AddItem(item, 1, false, sync: false, returnRaw: true);
            }

            if (i != null) syncItems.Add(i);

            gachaItem.GachaItem_ = new Item
            {
                ItemId = (uint)item,
                Num = 1,
                Level = 1,
                Rank = 1
            };

            var tokenItem = new ItemList();
            if (dirt > 0)
            {
                var it = await Player.InventoryManager!.AddItem(251, dirt, false, sync: false, returnRaw: true);
                if (it != null)
                {
                    var oldItem = syncItems.Find(x => x.ItemId == 251);
                    if (oldItem == null)
                        syncItems.Add(it);
                    else
                        oldItem.Count = it.Count;
                }

                tokenItem.ItemList_.Add(new Item
                {
                    ItemId = 251,
                    Num = (uint)dirt
                });
            }

            if (star > 0)
            {
                var it = await Player.InventoryManager!.AddItem(252, star, false, sync: false, returnRaw: true);
                if (it != null)
                {
                    var oldItem = syncItems.Find(x => x.ItemId == 252);
                    if (oldItem == null)
                        syncItems.Add(it);
                    else
                        oldItem.Count = it.Count;
                }

                tokenItem.ItemList_.Add(new Item
                {
                    ItemId = 252,
                    Num = (uint)star
                });
            }

            gachaItem.TokenItem = tokenItem;

            gachaItem.TransferItemList ??= new ItemList();

            gachaItems.Add(gachaItem);
        }

        await Player.SendPacket(new PacketPlayerSyncScNotify(syncItems));
        var proto = new DoGachaScRsp
        {
            GachaId = (uint)bannerId,
            GachaNum = (uint)times
        };
        proto.GachaItemList.AddRange(gachaItems);

        return proto;
    }

    public GetGachaInfoScRsp ToProto()
    {
        var proto = new GetGachaInfoScRsp
        {
            GachaRandom = (uint)Random.Shared.Next(1000, 1999)
        };
        foreach (var banner in GameData.BannersConfig.Banners) proto.GachaInfoList.Add(banner.ToInfo(GetGoldAvatars()));
        return proto;
    }
}