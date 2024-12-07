using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.AdventureAbility;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue.Buff;
using EggLink.DanhengServer.GameServer.Game.Rogue.Event;
using EggLink.DanhengServer.GameServer.Game.Rogue.Miracle;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using LineupInfo = EggLink.DanhengServer.Database.Lineup.LineupInfo;

namespace EggLink.DanhengServer.GameServer.Game.Rogue;

public abstract class BaseRogueInstance(PlayerInstance player, RogueSubModeEnum rogueSubMode, int rogueBuffType)
{
    public PlayerInstance Player { get; set; } = player;
    public LineupInfo? CurLineup { get; set; }
    public RogueSubModeEnum RogueSubMode { get; set; } = rogueSubMode;
    public int RogueType { get; set; } = 100;
    public int CurReviveCost { get; set; } = 80;
    public int CurRerollCost { get; set; } = 30;
    public int BaseRerollCount { get; set; } = 1;
    public int BaseRerollFreeCount { get; set; } = 0;
    public int CurMoney { get; set; } = 100;
    public int CurDestroyCount { get; set; }
    public int AeonId { get; set; } = 0;
    public int RogueBuffType { get; set; } = rogueBuffType;
    public bool IsWin { get; set; } = false;
    public List<RogueBuffInstance> RogueBuffs { get; set; } = [];
    public Dictionary<int, RogueMiracleInstance> RogueMiracles { get; set; } = [];

    public SortedDictionary<int, RogueActionInstance> RogueActions { get; set; } = []; // queue_position -> action
    public int CurActionQueuePosition { get; set; } = 0;
    public int CurEventUniqueId { get; set; } = 100;

    public int CurAeonBuffCount { get; set; } = 0;
    public int CurAeonEnhanceCount { get; set; } = 0;
    public bool AeonBuffPending { get; set; } // prevent multiple aeon buff

    public RogueEventManager? EventManager { get; set; }

    #region Buffs

    public virtual async ValueTask RollBuff(int amount)
    {
        await RollBuff(amount, 100005);
    }

    public virtual async ValueTask RollBuff(int amount, int buffGroupId, int buffHintType = 1, bool isReforge = false)
    {
        var buffGroup = GameData.RogueBuffGroupData[buffGroupId];
        var buffList = RogueSubMode == RogueSubModeEnum.TournRogue
            ? (buffGroup as RogueTournBuffGroupExcel)!.BuffList.Select(x => x)
            : (buffGroup as RogueBuffGroupExcel)!.BuffList;
        var actualBuffList = buffList.Where(buff => !RogueBuffs.Exists(x => x.BuffExcel.MazeBuffID == buff.MazeBuffID))
            .ToList();

        if (actualBuffList.Count == 0) return; // no buffs to roll

        for (var i = 0; i < amount; i++)
        {
            var menu = new RogueBuffSelectMenu(this)
            {
                CurCount = 1,
                TotalCount = 1
            };
            menu.RollBuff(actualBuffList);
            menu.HintId = buffHintType;
            var action = menu.GetActionInstance();
            action.IsReforge = isReforge;
            RogueActions.Add(action.QueuePosition, action);

            await UpdateMenu();
        }
    }

    public virtual async ValueTask<RogueCommonActionResult?> AddBuff(int buffId, int level = 1,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Dialogue,
        RogueCommonActionResultDisplayType displayType = RogueCommonActionResultDisplayType.Single,
        bool updateMenu = true, bool notify = true)
    {
        if (RogueBuffs.Exists(x => x.BuffExcel.MazeBuffID == buffId)) return null;
        GameData.RogueBuffData.TryGetValue(buffId * 100 + level, out var excel);
        if (excel == null) return null;
        if (CurAeonBuffCount > 0) // check if aeon buff exists
            if (excel is RogueBuffExcel { IsAeonBuff: true })
                return null;
        var buff = new RogueBuffInstance(buffId, level);
        RogueBuffs.Add(buff);
        var result = buff.ToResultProto(source);

        if (notify)
            await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, result, displayType));

        if (updateMenu) await UpdateMenu();

        return result;
    }

    public virtual async ValueTask<RogueCommonActionResult?> RemoveBuff(int buffId,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Dialogue,
        RogueCommonActionResultDisplayType displayType = RogueCommonActionResultDisplayType.Single,
        bool updateMenu = true, bool notify = true)
    {
        var buff = RogueBuffs.Find(x => x.BuffExcel.MazeBuffID == buffId);
        if (buff == null) return null; // buff not found
        RogueBuffs.Remove(buff);
        var result = buff.ToRemoveResultProto(source);

        if (notify)
            await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, result, displayType));

        if (updateMenu) await UpdateMenu();

        return result;
    }

    public virtual async ValueTask AddBuffList(List<BaseRogueBuffExcel> excel)
    {
        List<RogueCommonActionResult> resultList = [];
        foreach (var buff in excel)
        {
            var res = await AddBuff(buff.MazeBuffID, buff.MazeBuffLevel,
                displayType: RogueCommonActionResultDisplayType.Multi, updateMenu: false,
                notify: false);
            if (res != null) resultList.Add(res);
        }

        await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, resultList,
            RogueCommonActionResultDisplayType.Multi));

        await UpdateMenu();
    }

    public virtual async ValueTask EnhanceBuff(int buffId,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Dialogue)
    {
        var buff = RogueBuffs.Find(x => x.BuffExcel.MazeBuffID == buffId);
        if (buff != null)
        {
            GameData.RogueBuffData.TryGetValue(buffId * 100 + buff.BuffLevel + 1,
                out var excel); // make sure the next level exists
            if (excel != null)
            {
                buff.BuffLevel++;
                await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode,
                    buff.ToResultProto(source), RogueCommonActionResultDisplayType.Single));
            }
        }
    }

    public virtual List<RogueBuffInstance> GetRogueBuffInGroup(int groupId)
    {
        var group = GameData.RogueBuffGroupData[groupId];
        return RogueBuffs.FindAll(x => group.BuffList.Contains(x.BuffExcel));
    }

    public virtual async ValueTask HandleBuffSelect(int buffId, int location)
    {
        if (RogueActions.Count == 0) return;

        var action = RogueActions.First().Value;
        if (action.RogueBuffSelectMenu != null)
        {
            var buff = action.RogueBuffSelectMenu.Buffs.Find(x => x.MazeBuffID == buffId);
            if (buff != null) // check if buff is in the list
            {
                if (RogueBuffs.Exists(x => x.BuffExcel.MazeBuffID == buffId)) // check if buff already exists
                {
                    // enhance
                    await EnhanceBuff(buffId, RogueCommonActionResultSourceType.Select);
                }
                else
                {
                    var instance = new RogueBuffInstance(buff.MazeBuffID, buff.MazeBuffLevel);
                    RogueBuffs.Add(instance);
                    await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode,
                        instance.ToResultProto(RogueCommonActionResultSourceType.Select)));
                }
            }

            RogueActions.Remove(action.QueuePosition);
            if (action.RogueBuffSelectMenu.IsAeonBuff) AeonBuffPending = false; // aeon buff added
        }

        await UpdateMenu();

        await Player.SendPacket(new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, location, true));
    }

    public virtual async ValueTask HandleBuffReforgeSelect(int buffId, int location)
    {
        if (RogueActions.Count == 0) return;

        var action = RogueActions.First().Value;
        if (action.RogueBuffSelectMenu != null)
        {
            var buff = action.RogueBuffSelectMenu.Buffs.Find(x => x.MazeBuffID == buffId);
            if (buff != null) // check if buff is in the list
            {
                if (RogueBuffs.Exists(x => x.BuffExcel.MazeBuffID == buffId)) // check if buff already exists
                {
                    // enhance
                    await EnhanceBuff(buffId, RogueCommonActionResultSourceType.Select);
                }
                else
                {
                    var instance = new RogueBuffInstance(buff.MazeBuffID, buff.MazeBuffLevel);
                    RogueBuffs.Add(instance);
                    await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode,
                        instance.ToResultProto(RogueCommonActionResultSourceType.Select)));
                }
            }

            RogueActions.Remove(action.QueuePosition);
            if (action.RogueBuffSelectMenu.IsAeonBuff) AeonBuffPending = false; // aeon buff added
        }

        await UpdateMenu();

        await Player.SendPacket(
            new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, location, reforgeBuff: true));
    }

    public virtual async ValueTask HandleRerollBuff(int location)
    {
        if (RogueActions.Count == 0) return;
        var action = RogueActions.First().Value;
        if (action.RogueBuffSelectMenu != null)
        {
            await action.RogueBuffSelectMenu.RerollBuff(); // reroll
            await Player.SendPacket(
                new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, location,
                    menu: action.RogueBuffSelectMenu));
        }
    }

    public virtual void HandleMazeBuffModifier(AdventureModifierConfig config, MazeBuff buff)
    {
        var task = config.OnBeforeBattle;

        foreach (var info in task)
        {
            if (!info.Type.Replace("RPG.GameCore.", "").StartsWith("SetDynamicValueBy")) continue;
            var key = info.Type.Replace("RPG.GameCore.SetDynamicValueBy", "");
            var value = key switch
            {
                "ItemNum" => CurMoney,
                "RogueMiracleNum" => RogueMiracles.Count,
                "RogueBuffNumWithType" => RogueBuffs.Count,
                _ => 0
            };

            key = key switch
            {
                "ItemNum" => "ItemNumber",
                "RogueMiracleNum" => "RogueMiracleNumber",
                "RogueBuffNumWithType" => "RogueBuffNumberWithType",
                _ => key
            };

            buff.DynamicValues.Add(key, value);
        }
    }

    #endregion

    #region Money

    public async ValueTask CostMoney(int amount, int displayType = 0)
    {
        CurMoney -= amount;
        await Player.SendPacket(new PacketSyncRogueCommonVirtualItemInfoScNotify(this));

        await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode,
            new RogueCommonActionResult
            {
                Source = RogueCommonActionResultSourceType.Dialogue,
                RogueAction = new RogueCommonActionResultData
                {
                    RemoveItemList = new RogueCommonMoney
                    {
                        Num = (uint)amount,
                        DisplayType = (uint)displayType
                    }
                }
            }, RogueCommonActionResultDisplayType.Single));
    }

    public async ValueTask GainMoney(int amount, int displayType = 1)
    {
        CurMoney += amount;
        await Player.SendPacket(new PacketSyncRogueCommonVirtualItemInfoScNotify(this));
        await Player.SendPacket(new PacketScenePlaneEventScNotify(new ItemData
        {
            ItemId = 31,
            Count = amount
        }));

        await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode,
            new RogueCommonActionResult
            {
                RogueAction = new RogueCommonActionResultData
                {
                    GetItemList = new RogueCommonMoney
                    {
                        Num = (uint)amount,
                        DisplayType = (uint)displayType
                    }
                }
            }, RogueCommonActionResultDisplayType.Single));
    }

    #endregion


    #region Miracles

    public virtual async ValueTask RollMiracle(int amount, int groupId = 10002)
    {
        GameData.RogueMiracleGroupData.TryGetValue(groupId, out var group);
        if (group == null) return;

        for (var i = 0; i < amount; i++)
        {
            var list = new List<int>();

            foreach (var miracle in group)
            {
                if (RogueMiracles.ContainsKey(miracle)) continue;
                list.Add(miracle);
            }

            if (list.Count == 0) return;

            var menu = new RogueMiracleSelectMenu(this);
            menu.RollMiracle(list);
            var action = menu.GetActionInstance();
            RogueActions.Add(action.QueuePosition, action);
        }

        await UpdateMenu();
    }

    public virtual async ValueTask HandleMiracleSelect(uint miracleId, int location)
    {
        if (RogueActions.Count == 0) return;

        var action = RogueActions.First().Value;
        if (action.RogueMiracleSelectMenu != null)
        {
            var miracle = action.RogueMiracleSelectMenu.Results.Find(x => x == miracleId);
            if (miracle != 0) await AddMiracle((int)miracle);
            RogueActions.Remove(action.QueuePosition);
        }

        await UpdateMenu();

        await Player.SendPacket(
            new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, location, selectMiracle: true));
    }

    public virtual async ValueTask AddMiracle(int miracleId)
    {
        if (RogueMiracles.ContainsKey(miracleId)) return;

        GameData.RogueMiracleData.TryGetValue(miracleId, out var excel);
        if (excel == null) return;

        var miracle = new RogueMiracleInstance(this, miracleId);
        RogueMiracles.Add(miracleId, miracle);
        await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, miracle.ToGetResult(),
            RogueCommonActionResultDisplayType.Single));
    }

    #endregion

    #region Actions

    public virtual async ValueTask HandleBonusSelect(int bonusId, int location)
    {
        if (RogueActions.Count == 0) return;

        var action = RogueActions.First().Value;

        // TODO: handle bonus
        GameData.RogueBonusData.TryGetValue(bonusId, out var bonus);
        if (bonus != null) await TriggerEvent(null, bonus.BonusEvent);

        RogueActions.Remove(action.QueuePosition);
        await UpdateMenu();

        await Player.SendPacket(
            new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, location, selectBonus: true));
    }

    public virtual async ValueTask UpdateMenu(int position = 0)
    {
        if (RogueActions.Count > 0)
        {
            if (position == 0)
            {
                var action = RogueActions.Values.First();
                action.GetSelectMenu()?.Roll();
                await Player.SendPacket(
                    new PacketSyncRogueCommonPendingActionScNotify(action, RogueSubMode));
            }
            else
            {
                var action = RogueActions[position];
                action.GetSelectMenu()?.Roll();
                await Player.SendPacket(
                    new PacketSyncRogueCommonPendingActionScNotify(action, RogueSubMode));
            }
        }
    }

    #endregion

    #region Handlers

    public virtual void OnBattleStart(BattleInstance battle)
    {
        foreach (var miracle in RogueMiracles.Values) miracle.OnStartBattle(battle);

        foreach (var buff in RogueBuffs) buff.OnStartBattle(battle);
    }

    public abstract ValueTask OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req);

    public virtual async ValueTask OnPropDestruct(EntityProp prop)
    {
        if (!prop.Excel.IsHpRecover && !prop.Excel.IsMpRecover)
            // gain money
            await GainMoney(Random.Shared.Next(10, 30));

        CurDestroyCount++;
    }

    #endregion

    #region Events

    public async ValueTask TriggerEvent(RogueEventInstance? rogueEvent, int eventId)
    {
        if (EventManager == null) return;
        await EventManager.TriggerEvent(rogueEvent, eventId);
    }

    public async ValueTask<RogueEventInstance> GenerateEvent(RogueNpc npc)
    {
        RogueNPCExcel? dialogue;
        do
        {
            dialogue = GameData.RogueNPCData.Values.ToList().RandomElement();
            if (dialogue.NPCJsonPath.Contains("RogueNPC_230") && RogueSubMode != RogueSubModeEnum.TournRogue)
                // skip because it's a tourn rogue event
                dialogue = null;
            else if (!dialogue.NPCJsonPath.Contains("RogueNPC_230") && RogueSubMode == RogueSubModeEnum.TournRogue)
                // skip because it's not a tourn rogue event
                dialogue = null;
        } while (dialogue == null || (!dialogue.CanUseInVer(RogueType) &&
                                      dialogue.RogueNpcConfig?.DialogueType == RogueDialogueTypeEnum.Event));

        var instance = new RogueEventInstance(dialogue, npc, CurEventUniqueId++);
        if (EventManager == null) return instance;
        await EventManager.AddEvent(instance);

        return instance;
    }

    public async ValueTask HandleSelectOption(int eventUniqueId, int optionId)
    {
        var entity = Player.SceneInstance!.Entities.Values.FirstOrDefault(x =>
            x is RogueNpc npc && npc.RogueEvent?.EventUniqueId == eventUniqueId);
        if (entity is not RogueNpc npc) return;

        if (EventManager == null) return;
        await EventManager.SelectOption(npc.RogueEvent!, optionId);
    }

    public async ValueTask HandleFinishDialogueGroup(int eventUniqueId)
    {
        var entity = Player.SceneInstance!.Entities.Values.FirstOrDefault(x =>
            x is RogueNpc npc && npc.RogueEvent?.EventUniqueId == eventUniqueId);
        if (entity is not RogueNpc npc) return;

        await EventManager!.FinishEvent(npc.RogueEvent!);
    }

    public async ValueTask HandleNpcDisappear(int entityId)
    {
        Player.SceneInstance!.Entities.TryGetValue(entityId, out var entity);
        if (entity is not RogueNpc npc) return;

        await EventManager!.NpcDisappear(npc.RogueEvent!);
    }

    #endregion

    #region Serialization

    public RogueBuffEnhanceInfoList ToEnhanceInfo()
    {
        var proto = new RogueBuffEnhanceInfoList();

        foreach (var buff in RogueBuffs) proto.EnhanceInfoList.Add(buff.ToEnhanceProto());

        return proto;
    }

    public ChessRogueBuffEnhanceList ToChessEnhanceInfo()
    {
        var proto = new ChessRogueBuffEnhanceList();

        foreach (var buff in RogueBuffs) proto.EnhanceInfoList.Add(buff.ToChessEnhanceProto());

        return proto;
    }

    #endregion
}