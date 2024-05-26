using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Rogue.Buff;
using EggLink.DanhengServer.Game.Rogue.Event;
using EggLink.DanhengServer.Game.Rogue.Miracle;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using EggLink.DanhengServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.Game.Rogue
{
    public class BaseRogueInstance(PlayerInstance player, int rogueVersionId, int rogueBuffType)
    {
        public PlayerInstance Player { get; set; } = player;
        public Database.Lineup.LineupInfo? CurLineup { get; set; }
        public int RogueVersionId { get; set; } = rogueVersionId;
        public int CurReviveCost { get; set; } = 80;
        public int CurRerollCost { get; set; } = 30;
        public int BaseRerollCount { get; set; } = 1;
        public int BaseRerollFreeCount { get; set; } = 0;
        public int CurMoney { get; set; } = 100;
        public int CurDestroyCount { get; set; } = 0;
        public int AeonId { get; set; } = 0;
        public int RogueBuffType { get; set; } = rogueBuffType;
        public bool IsWin { get; set; } = false;
        public List<RogueBuffInstance> RogueBuffs { get; set; } = [];
        public Dictionary<int, RogueMiracleInstance> RogueMiracles { get; set; } = [];

        public SortedDictionary<int, RogueActionInstance> RogueActions { get; set; } = [];  // queue_position -> action
        public int CurActionQueuePosition { get; set; } = 0;
        public int CurEventUniqueID { get; set; } = 100;

        public int CurAeonBuffCount { get; set; } = 0;
        public int CurAeonEnhanceCount { get; set; } = 0;
        public bool AeonBuffPending { get; set; } = false;  // prevent multiple aeon buff

        public RogueEventManager? EventManager { get; set; }

        #region Buffs

        public virtual void RollBuff(int amount)
        {
            RollBuff(amount, 100005);
        }

        public virtual void RollBuff(int amount, int buffGroupId, int buffHintType = 1)
        {
            var buffGroup = GameData.RogueBuffGroupData[buffGroupId];
            var buffList = buffGroup.BuffList;
            var actualBuffList = new List<RogueBuffExcel>();
            foreach (var buff in buffList)
            {
                if (!RogueBuffs.Exists(x => x.BuffExcel.MazeBuffID == buff.MazeBuffID))
                {
                    actualBuffList.Add(buff);
                }
            }

            if (actualBuffList.Count == 0)
            {
                return;  // no buffs to roll
            }

            for (int i = 0; i < amount; i++)
            {
                var menu = new RogueBuffSelectMenu(this)
                {
                    CurCount = i + 1,
                    TotalCount = amount,
                };
                menu.RollBuff(actualBuffList);
                menu.HintId = buffHintType;
                var action = menu.GetActionInstance();
                RogueActions.Add(action.QueuePosition, action);
            }

            UpdateMenu();
        }

        public virtual RogueCommonActionResult? AddBuff(int buffId, int level = 1, RogueActionSource source = RogueActionSource.RogueCommonActionResultSourceTypeDialogue, RogueActionDisplayType displayType = RogueActionDisplayType.RogueCommonActionResultDisplayTypeSingle, bool updateMenu = true, bool notify = true)
        {
            if (RogueBuffs.Exists(x => x.BuffExcel.MazeBuffID == buffId))
            {
                return null;
            }
            GameData.RogueBuffData.TryGetValue(buffId * 100 + level, out var excel);
            if (excel == null) return null;
            if (CurAeonBuffCount > 0)  // check if aeon buff exists
            {
                if (excel.IsAeonBuff)
                {
                    return null;
                }
            }
            var buff = new RogueBuffInstance(buffId, level);
            RogueBuffs.Add(buff);
            var result = buff.ToResultProto(source);

            if (notify)
            {
                Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueVersionId, result, displayType));
            }

            if (updateMenu)
            {
                UpdateMenu();
            }

            return result;
        }

        public virtual void AddBuffList(List<RogueBuffExcel> excel)
        {
            List<RogueCommonActionResult> resultList = [];
            foreach (var buff in excel)
            {
                var res = AddBuff(buff.MazeBuffID, buff.MazeBuffLevel, displayType: RogueActionDisplayType.RogueCommonActionResultDisplayTypeMulti, updateMenu: false, notify: false);
                if (res != null)
                {
                    resultList.Add(res);
                }
            }

            Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueVersionId, resultList, RogueActionDisplayType.RogueCommonActionResultDisplayTypeMulti));

            UpdateMenu();
        }

        public virtual void EnhanceBuff(int buffId, RogueActionSource source = RogueActionSource.RogueCommonActionResultSourceTypeDialogue)
        {
            var buff = RogueBuffs.Find(x => x.BuffExcel.MazeBuffID == buffId);
            if (buff != null)
            {
                GameData.RogueBuffData.TryGetValue(buffId * 100 + buff.BuffLevel + 1, out var excel);  // make sure the next level exists
                if (excel != null)
                {
                    buff.BuffLevel++;
                    Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueVersionId, buff.ToResultProto(source), RogueActionDisplayType.RogueCommonActionResultDisplayTypeSingle));
                }
            }
        }

        public virtual List<RogueBuffInstance> GetRogueBuffInGroup(int groupId)
        {
            var group = GameData.RogueBuffGroupData[groupId];
            return RogueBuffs.FindAll(x => group.BuffList.Contains(x.BuffExcel));
        }

        public virtual void HandleBuffSelect(int buffId)
        {
            if (RogueActions.Count == 0)
            {
                return;
            }

            var action = RogueActions.First().Value;
            if (action.RogueBuffSelectMenu != null)
            {
                var buff = action.RogueBuffSelectMenu.Buffs.Find(x => x.MazeBuffID == buffId);
                if (buff != null)  // check if buff is in the list
                {
                    if (RogueBuffs.Exists(x => x.BuffExcel.MazeBuffID == buffId))  // check if buff already exists
                    {
                        // enhance
                        EnhanceBuff(buffId, RogueActionSource.RogueCommonActionResultSourceTypeSelect);
                    }
                    else
                    {
                        var instance = new RogueBuffInstance(buff.MazeBuffID, buff.MazeBuffLevel);
                        RogueBuffs.Add(instance);
                        Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueVersionId, instance.ToResultProto(RogueActionSource.RogueCommonActionResultSourceTypeSelect)));
                    }
                }
                RogueActions.Remove(action.QueuePosition);
                if (action.RogueBuffSelectMenu.IsAeonBuff)
                {
                    AeonBuffPending = false;  // aeon buff added
                }
            }

            UpdateMenu();

            Player.SendPacket(new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, selectBuff: true));
        }

        public virtual void HandleRerollBuff()
        {
            if (RogueActions.Count == 0)
            {
                return;
            }
            var action = RogueActions.First().Value;
            if (action.RogueBuffSelectMenu != null)
            {
                action.RogueBuffSelectMenu.RerollBuff();  // reroll
                Player.SendPacket(new PacketHandleRogueCommonPendingActionScRsp(RogueVersionId, menu: action.RogueBuffSelectMenu));
            }
        }

        #endregion

        #region Money

        public void CostMoney(int amount, RogueActionDisplayType displayType = RogueActionDisplayType.RogueCommonActionResultDisplayTypeNone)
        {
            CurMoney -= amount;
            Player.SendPacket(new PacketSyncRogueVirtualItemScNotify(this));

            Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueVersionId, new RogueCommonActionResult()
            {
                Source = RogueActionSource.RogueCommonActionResultSourceTypeDialogue,
                RogueAction = new()
                {
                    RemoveItemList = new()
                    {
                        Num = (uint)amount,
                        DisplayType = (uint)displayType + 1,
                    },
                },
            }, displayType));
        }

        public void GainMoney(int amount, int displayType = 2, RogueActionDisplayType display = RogueActionDisplayType.RogueCommonActionResultDisplayTypeNone)
        {
            CurMoney += amount;
            Player.SendPacket(new PacketSyncRogueVirtualItemScNotify(this));
            Player.SendPacket(new PacketScenePlaneEventScNotify(new Database.Inventory.ItemData()
            {
                ItemId = 31,
                Count = amount,
            }));

            Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueVersionId, new RogueCommonActionResult()
            {
                Source = RogueActionSource.RogueCommonActionResultSourceTypeDialogue,
                RogueAction = new()
                {
                    GetItemList = new()
                    {
                        Num = (uint)amount,
                        DisplayType = (uint)displayType,
                    },
                },
            }, display));
        }

        #endregion


        #region Miracles

        public virtual void RollMiracle(int amount, int groupId = 10002)
        {
            GameData.RogueMiracleGroupData.TryGetValue(groupId, out var group);
            if (group == null) return;

            for (int i = 0; i < amount; i++)
            {
                var list = new List<int>();

                foreach (var miracle in group)
                {
                    if (RogueMiracles.ContainsKey(miracle))
                    {
                        continue;
                    }
                    list.Add(miracle);
                }

                if (list.Count == 0) return;

                var menu = new RogueMiracleSelectMenu(this);
                menu.RollMiracle(list);
                var action = menu.GetActionInstance();
                RogueActions.Add(action.QueuePosition, action);
            }

            UpdateMenu();
        }

        public virtual void HandleMiracleSelect(uint miracleId)
        {
            if (RogueActions.Count == 0)
            {
                return;
            }

            var action = RogueActions.First().Value;
            if (action.RogueMiracleSelectMenu != null)
            {
                var miracle = action.RogueMiracleSelectMenu.Results.Find(x => x == miracleId);
                if (miracle != 0)
                {
                    AddMiracle((int)miracle);
                }
                RogueActions.Remove(action.QueuePosition);
            }

            UpdateMenu();

            Player.SendPacket(new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, selectMiracle: true));
        }

        public virtual void AddMiracle(int miracleId)
        {
            if (RogueMiracles.ContainsKey(miracleId))
            {
                return;
            }

            GameData.RogueMiracleData.TryGetValue(miracleId, out var excel);
            if (excel == null) return;

            var miracle = new RogueMiracleInstance(this, miracleId);
            RogueMiracles.Add(miracleId, miracle);
            Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueVersionId, miracle.ToGetResult(), RogueActionDisplayType.RogueCommonActionResultDisplayTypeSingle));
        }

        #endregion

        #region Actions

        public virtual void HandleBonusSelect(int bonusId)
        {
            if (RogueActions.Count == 0)
            {
                return;
            }

            var action = RogueActions.First().Value;

            // TODO: handle bonus
            GameData.RogueBonusData.TryGetValue(bonusId, out var bonus);
            if (bonus != null)
            {
                TriggerEvent(null, bonus.BonusEvent);
            }

            RogueActions.Remove(action.QueuePosition);
            UpdateMenu();

            Player.SendPacket(new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, selectBonus: true));
        }

        public virtual void UpdateMenu()
        {
            if (RogueActions.Count > 0)
            {
                Player.SendPacket(new PacketSyncRogueCommonPendingActionScNotify(RogueActions.First().Value, RogueVersionId));
            }
        }

        #endregion

        #region Handlers

        public virtual void OnBattleStart(BattleInstance battle)
        {
            foreach (var miracle in RogueMiracles.Values)
            {
                miracle.OnStartBattle(battle);
            }

            foreach (var buff in RogueBuffs)
            {
                buff.OnStartBattle(battle);
            }
        }

        public virtual void OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req)
        {
            foreach (var miracle in RogueMiracles.Values)
            {
                miracle.OnEndBattle(battle);
            }
        }

        public virtual void OnPropDestruct(EntityProp prop)
        {
            if (!prop.Excel.IsHpRecover && !prop.Excel.IsMpRecover)
            {
                // gain money
                GainMoney(Random.Shared.Next(10, 30));
            }

            CurDestroyCount++;
        }

        #endregion

        #region Events

        public void TriggerEvent(RogueEventInstance? rogueEvent, int eventId)
        {
            EventManager?.TriggerEvent(rogueEvent, eventId);
        }

        public RogueEventInstance GenerateEvent(RogueNpc npc)
        {
            RogueNPCDialogueExcel? dialogue;
            do
            {
                dialogue = GameData.RogueNPCDialogueData.Values.ToList().RandomElement();
            } while (dialogue == null || !dialogue.CanUseInCommon());

            var instance = new RogueEventInstance(dialogue, npc, CurEventUniqueID++);
            EventManager?.AddEvent(instance);

            return instance;
        }

        public void HandleSelectOption(int eventId, int entityId)
        {
            var entity = Player.SceneInstance!.Entities[entityId];
            if (entity is not RogueNpc npc)
            {
                return;
            }

            EventManager?.SelectOption(npc.RogueEvent!, eventId);
        }

        public void HandleFinishDialogueGroup(int entityId)
        {
            Player.SceneInstance!.Entities.TryGetValue(entityId, out var entity);
            if (entity == null || entity is not RogueNpc npc)
            {
                return;
            }

            EventManager?.FinishEvent(npc.RogueEvent!);
        }

        public void HandleNpcDisappear(int entityId)
        {
            Player.SceneInstance!.Entities.TryGetValue(entityId, out var entity);
            if (entity == null || entity is not RogueNpc npc)
            {
                return;
            }

            EventManager?.NpcDisappear(npc.RogueEvent!);
        }

        #endregion

        #region Serialization

        public RogueBuffEnhanceInfo ToEnhanceInfo()
        {
            var proto = new RogueBuffEnhanceInfo();

            foreach (var buff in RogueBuffs)
            {
                proto.EnhanceInfo.Add(buff.ToEnhanceProto());
            }

            return proto;
        }

        public ChessRogueBuffEnhanceInfo ToChessEnhanceInfo()
        {
            var proto = new ChessRogueBuffEnhanceInfo();

            foreach (var buff in RogueBuffs)
            {
                proto.EnhanceInfo.Add(buff.ToChessEnhanceProto());
            }

            return proto;
        }

        #endregion
    }
}
