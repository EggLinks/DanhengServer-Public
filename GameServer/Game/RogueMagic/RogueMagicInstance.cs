using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue;
using EggLink.DanhengServer.GameServer.Game.Rogue.Event;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.Adventure;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.MagicUnit;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.Scene;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.Scepter;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic;

public class RogueMagicInstance : BaseRogueInstance
{
    #region Initializer

    public RogueMagicInstance(PlayerInstance player, int areaId, List<int> difficultyIds, int styleType) : base(player,
        RogueSubModeEnum.MagicRogue, 0)
    {
        // generate levels
        AreaExcel = GameData.RogueMagicAreaData.GetValueOrDefault(areaId) ??
                    throw new Exception("Invalid area id"); // wont be null because of validation in RogueMagicManager

        foreach (var index in Enumerable.Range(1, AreaExcel.LayerIDList.Count))
        {
            var layerId = AreaExcel.LayerIDList[index - 1];
            var levelInstance = new RogueMagicLevelInstance(index, layerId,
                GameData.RogueMagicLayerIdRoomCountDict.GetValueOrDefault(layerId));
            Levels.Add(levelInstance.LayerId, levelInstance);
        }

        CurLayerId = AreaExcel.LayerIDList[0];
        EventManager = new RogueEventManager(player, this);

        BaseRerollCount = 0;

        foreach (var id in difficultyIds)
        {
            GameData.RogueMagicDifficultyCompData.TryGetValue(id, out var excel);
            if (excel != null)
                DifficultyCompExcels.Add(excel);
        }

        foreach (var id in AreaExcel.DifficultyIDList) // need to find a better way to get difficulty excels
        {
            GameData.RogueTournDifficultyData.TryGetValue(1000 + id, out var excel);
            if (excel != null)
                DifficultyExcels.Add(excel);
        }

        StyleType = (RogueMagicStyleTypeEnum)styleType;

        var t = RollScepter(1, 1);
        t.AsTask().Wait();
    }

    #endregion

    #region Adventure

    public async ValueTask HandleStopWolfGunAdventure(List<int> targetIndex, RogueMagicAdventureInstance instance)
    {
        if (instance.WolfGunTargets.Count == 0) return;

        var result = (from index in targetIndex
            where index >= 0 && index < instance.WolfGunTargets.Count
            select instance.WolfGunTargets[index]).ToList();

        foreach (var target in result)
            if (target.IsMoney)
            {
                // money
                await GainMoney(target.TargetId, 2);
            }
            else if (target.IsMiracle)
            {
                // miracle
                await AddMiracle(target.TargetId);
            }
            else if (target.IsRuanmei)
            {
                // ruanmei
                var unitExcels = GameData.RogueMagicUnitData.Values
                    .Where(x => x.MagicUnitLevel == 1 && x.MagicUnitType != RogueMagicMountTypeEnum.Active).ToList();
                var toAddExcel = Enumerable.Range(0, 10).Select(unused => unitExcels.RandomElement()).ToList();

                await AddMagicUnits(toAddExcel, RogueCommonActionResultSourceType.None);
            }
    }

    #endregion

    #region Properties

    public RogueMagicAreaExcel AreaExcel { get; set; }
    public List<RogueMagicDifficultyCompExcel> DifficultyCompExcels { get; set; } = [];
    public Dictionary<int, RogueMagicLevelInstance> Levels { get; set; } = [];
    public List<RogueTournDifficultyExcel> DifficultyExcels { get; set; } = []; // for battle

    public int CurLayerId { get; set; }
    public RogueMagicLevelInstance? CurLevel => Levels.GetValueOrDefault(CurLayerId);

    public RogueMagicLevelStatus LevelStatus { get; set; } = RogueMagicLevelStatus.Processing;
    public RogueMagicStyleTypeEnum StyleType { get; set; }

    public Dictionary<int, RogueScepterInstance> RogueScepters { get; set; } = [];
    public Dictionary<int, RogueMagicUnitInstance> RogueMagicUnits { get; set; } = [];
    public int CurMagicUnitUniqueId { get; set; } = 1;
    public int BasicRoundCnt { get; set; } = 2;
    public int ExtraRoundCnt { get; set; } = 2;

    public Dictionary<RogueMagicRoomTypeEnum, int> RoomTypeWeight { get; set; } = new()
    {
        { RogueMagicRoomTypeEnum.Battle, 15 },
        { RogueMagicRoomTypeEnum.Wealth, 4 },
        { RogueMagicRoomTypeEnum.Shop, 4 },
        { RogueMagicRoomTypeEnum.Event, 7 },
        { RogueMagicRoomTypeEnum.Adventure, 60 },
        { RogueMagicRoomTypeEnum.Reward, 5 },
        { RogueMagicRoomTypeEnum.Elite, 1 }
    };

    #endregion

    #region Scene

    public async ValueTask EnterNextLayer(int roomIndex, RogueMagicRoomTypeEnum type)
    {
        var curIndex = CurLevel?.LevelIndex ?? 0;
        if (curIndex == 3)
            // last layer
            return;

        CurLayerId = AreaExcel.LayerIDList[curIndex];
        await EnterRoom(1, type);
    }

    public async ValueTask EnterRoom(int roomIndex, RogueMagicRoomTypeEnum type)
    {
        if (CurLevel == null) return;

        //if (CurLevel.CurRoomIndex == roomIndex)
        //    // same room
        //    return;

        //if (CurLevel.CurRoomIndex + 1 != roomIndex) // only allow to enter next room
        //    // invalid room
        //    return;
        if (CurLevel.CurRoom != null)
            CurLevel.CurRoom.Status = RogueMagicRoomStatus.Finish;

        // enter room
        CurLevel.CurRoomIndex = roomIndex;
        CurLevel.CurRoom?.Init(type);

        // next room
        var next = CurLevel.Rooms.Find(x => x.RoomIndex == roomIndex + 1);
        if (next != null)
            next.Status = RogueMagicRoomStatus.Inited;

        // scene
        var entrance = CurLevel.CurRoom?.Config?.EntranceId ?? 0;
        var group = CurLevel.CurRoom?.Config?.AnchorGroup ?? 0;
        var anchor = CurLevel.CurRoom?.Config?.AnchorId ?? 1;

        // call event
        EventManager?.OnNextRoom();
        foreach (var miracle in RogueMiracles.Values) miracle.OnEnterNextRoom();

        await Player.EnterMissionScene(entrance, group, anchor, false);

        // sync
        await Player.SendPacket(new PacketRogueMagicLevelInfoUpdateScNotify(this, [CurLevel], [
            next?.RoomIndex ?? 0,
            (next?.RoomIndex ?? 0) - 1
        ]));
    }

    public async ValueTask QuitRogue()
    {
        Player.LineupManager?.SetExtraLineup(ExtraLineupType.LineupNone, []);

        var currentLineup = Player.LineupManager!.GetCurLineup()!;

        await Player.SendPacket(new PacketSyncLineupNotify(currentLineup));

        await Player.EnterMissionScene(801120102, 0, 0, false);

        Player.RogueMagicManager!.RogueMagicInstance = null;
    }

    #endregion

    #region Scepter

    public async ValueTask RollScepter(int amount, int level)
    {
        var scepterExcels = GameData.RogueMagicScepterData.Values
            .Where(x => !RogueScepters.ContainsKey(x.ScepterID) && x.ScepterLevel == level).ToList();

        for (var i = 0; i < amount; i++)
        {
            var menu = new RogueScepterSelectMenu(this);
            menu.SetScepterPool(scepterExcels);
            var action = menu.GetActionInstance();
            RogueActions.Add(action.QueuePosition, action);

            await UpdateMenu(action.QueuePosition);
        }
    }

    public async ValueTask HandleScepterSelect(RogueMagicScepter selectScepter, int location)
    {
        if (RogueActions.Count == 0) return;

        var action = RogueActions.First().Value;
        if (action.RogueScepterSelectMenu != null)
        {
            var scepterExcel = action.RogueScepterSelectMenu.Scepters.Find(x => x.ScepterID == selectScepter.ScepterId);
            if (scepterExcel != null) await AddScepter(scepterExcel);
            RogueActions.Remove(action.QueuePosition);
        }

        await UpdateMenu();

        await Player.SendPacket(
            new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, location, selectScepter: true));
    }

    public async ValueTask AddScepter(RogueMagicScepterExcel excel,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Select)
    {
        var scepter = new RogueScepterInstance(excel);
        RogueScepters.Add(excel.ScepterID, scepter);

        await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, scepter.ToGetInfo(source)));
    }

    public async ValueTask DressScepter(int scepterId, int slot, int unitUniqueId)
    {
        var scepter = RogueScepters.GetValueOrDefault(scepterId);
        if (scepter == null) return;

        var unit = RogueMagicUnits.GetValueOrDefault(unitUniqueId);
        if (unit == null) return;

        // add
        scepter.AddUnit(slot, unit);

        await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode,
            scepter.ToDressInfo(RogueCommonActionResultSourceType.None)));
    }

    #endregion

    #region MagicUnit

    public async ValueTask RollMagicUnit(int amount, int level, List<RogueMagicUnitCategoryEnum> categories)
    {
        var unitExcels = GameData.RogueMagicUnitData.Values.Where(x =>
            x.MagicUnitLevel == level && categories.Contains(x.MagicUnitCategory) &&
            x.MagicUnitType != RogueMagicMountTypeEnum.Active).ToList();

        for (var i = 0; i < amount; i++)
        {
            var menu = new RogueMagicUnitSelectMenu(this);
            menu.SetPool(unitExcels);
            var action = menu.GetActionInstance();
            RogueActions.Add(action.QueuePosition, action);

            await UpdateMenu(action.QueuePosition);
        }
    }

    public async ValueTask HandleMagicUnitSelect(RogueMagicGameUnit selectMagicUnit, int location)
    {
        if (RogueActions.Count == 0) return;

        var action = RogueActions.First().Value;
        if (action.RogueMagicUnitSelectMenu != null)
        {
            var unitExcel =
                action.RogueMagicUnitSelectMenu.MagicUnits.Find(x => x.MagicUnitID == selectMagicUnit.MagicUnitId);
            if (unitExcel != null) await AddMagicUnit(unitExcel);
            RogueActions.Remove(action.QueuePosition);
        }

        await UpdateMenu();

        await Player.SendPacket(
            new PacketHandleRogueCommonPendingActionScRsp(action.QueuePosition, location, selectMagicUnit: true));
    }

    public async ValueTask<RogueCommonActionResult> AddMagicUnit(RogueMagicUnitExcel excel,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Select,
        RogueCommonActionResultDisplayType display = RogueCommonActionResultDisplayType.None, bool sync = true,
        bool compose = true)
    {
        var unit = new RogueMagicUnitInstance(excel)
        {
            UniqueId = CurMagicUnitUniqueId++
        };
        RogueMagicUnits.Add(unit.UniqueId, unit);

        var res = unit.ToGetInfo(source);
        if (sync)
            await Player.SendPacket(
                new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, res, display));

        if (compose)
            await HandleAutoComposeUnit();

        return res;
    }

    public async ValueTask<RogueCommonActionResult?> RemoveMagicUnit(int uniqueId,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Select,
        RogueCommonActionResultDisplayType display = RogueCommonActionResultDisplayType.None, bool sync = true)
    {
        if (!RogueMagicUnits.Remove(uniqueId, out var unit)) return null;

        var res = unit.ToRemoveInfo(source);
        if (sync)
            await Player.SendPacket(
                new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, res, display));

        return res;
    }

    public async ValueTask HandleAutoComposeUnit()
    {
        Dictionary<int, Dictionary<int, List<RogueMagicUnitInstance>>> unitMap = [];
        foreach (var unitInstance in RogueMagicUnits.Values)
        {
            unitMap.TryAdd(unitInstance.Excel.MagicUnitID, []);
            unitMap[unitInstance.Excel.MagicUnitID].TryAdd(unitInstance.Excel.MagicUnitLevel, []);

            unitMap[unitInstance.Excel.MagicUnitID][unitInstance.Excel.MagicUnitLevel].Add(unitInstance);
        }

        foreach (var unitLevelDict in unitMap)
        foreach (var unitLevelMap in unitLevelDict.Value)
        {
            if (unitLevelMap.Key == 3) continue; // max level
            if (unitLevelMap.Value.Count < 3) continue; // cannot compose

            // remove
            List<RogueMagicUnitInstance> removeInstances = [];
            var addCount = unitLevelMap.Value.Count / 3;

            removeInstances.AddRange(Enumerable.Range(0, addCount * 3).Select(i => unitLevelMap.Value[i]));

            List<RogueCommonActionResult> resList = [];
            foreach (var rogueMagicUnitInstance in removeInstances)
            {
                var res = await RemoveMagicUnit(rogueMagicUnitInstance.UniqueId,
                    RogueCommonActionResultSourceType.MagicUnitCompose,
                    sync: false);

                if (res != null) resList.Add(res);
            }

            // get the dressed scepter
            List<RogueCommonActionResult> scepterResList = [];
            scepterResList.AddRange(from rogueMagicUnitInstance in removeInstances
                from scepter in RogueScepters
                where scepter.Value.DressedUnits.Any(x => x.Value.Remove(rogueMagicUnitInstance))
                select scepter.Value.ToDressInfo(RogueCommonActionResultSourceType.MagicUnitCompose));

            // send packet
            await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, scepterResList));
            await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, resList));

            // add new
            var excels = GameData.RogueMagicUnitData.Values.Where(x =>
                x.MagicUnitID == unitLevelDict.Key && x.MagicUnitLevel == unitLevelMap.Key + 1).ToList();

            if (excels.Count <= 0) continue;
            for (var i = 0; i < addCount; i++)
                await AddMagicUnit(excels.RandomElement(), RogueCommonActionResultSourceType.MagicUnitCompose,
                    compose: false);

            // select another
            await RollMagicUnit(addCount, unitLevelMap.Key, [excels.RandomElement().MagicUnitCategory]);
        }
    }

    public async ValueTask AddMagicUnits(List<RogueMagicUnitExcel> excels,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Select,
        RogueCommonActionResultDisplayType display = RogueCommonActionResultDisplayType.Multi)
    {
        List<RogueCommonActionResult> results = [];
        foreach (var excel in excels)
            results.Add(await AddMagicUnit(excel, source, RogueCommonActionResultDisplayType.None, false));

        await Player.SendPacket(new PacketSyncRogueCommonActionResultScNotify(RogueSubMode, results, display));
    }

    #endregion

    #region Rewrite

    public override async ValueTask<RogueCommonActionResult?> AddBuff(int buffId, int level = 1,
        RogueCommonActionResultSourceType source = RogueCommonActionResultSourceType.Dialogue,
        RogueCommonActionResultDisplayType displayType = RogueCommonActionResultDisplayType.Single,
        bool updateMenu = true,
        bool notify = true)
    {
        // this mode does not support buff
        await ValueTask.CompletedTask;
        return null;
    }

    public override async ValueTask RollBuff(int amount, int buffGroupId, int buffHintType = 1, bool isReforge = false)
    {
        // this mode does not support buff
        await ValueTask.CompletedTask;
    }

    #endregion

    #region Handlers

    public override void OnBattleStart(BattleInstance battle)
    {
        base.OnBattleStart(battle);
        if (DifficultyExcels.Count > 0)
        {
            var excel = DifficultyExcels.RandomElement();
            if (excel.LevelList.Count > 0)
                battle.CustomLevel = excel.LevelList.RandomElement();
        }

        battle.MagicInfo = new BattleRogueMagicInfo
        {
            ModifierContent = new BattleRogueMagicModifierInfo
            {
                RogueMagicBattleConst = 5
            },
            DetailInfo = new BattleRogueMagicDetailInfo
            {
                BattleMagicItemInfo = new BattleRogueMagicItemInfo
                {
                    BattleRoundCount = new BattleRogueMagicRoundCount
                    {
                        BattleStandardRoundLimit = (uint)BasicRoundCnt,
                        BattleExtraRoundLimit = (uint)ExtraRoundCnt
                    },
                    BattleScepterList = { RogueScepters.Select(x => x.Value.ToBattleScepterInfo()) }
                }
            }
        };
    }

    public async ValueTask HitMonsterInAdventure(List<EntityMonster> monsters)
    {
        var inst = CurLevel?.CurRoom?.AdventureInstance;
        if (inst != null && inst.Excel.AdventureType == RogueAdventureGameplayTypeEnum.RogueCaptureMonster)
        {
            inst.Score += 200;
            inst.RemainMonsterNum--;
            inst.CaughtMonsterNum++;

            await Player.SendPacket(new PacketSyncRogueAdventureRoomInfoScNotify(inst));
        }
    }

    public override async ValueTask OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req)
    {
        if (battle.BattleEndStatus != BattleEndStatus.BattleEndWin)
            // quit
            return;

        // extra round calc
        var usedCnt = req.Stt.RoundCnt;

        if (usedCnt > BasicRoundCnt)
        {
            var extraCnt = (int)usedCnt - BasicRoundCnt;
            if (extraCnt > ExtraRoundCnt) extraCnt = ExtraRoundCnt;

            ExtraRoundCnt -= extraCnt;
        }

        if (CurLevel!.CurRoom!.RoomType == RogueMagicRoomTypeEnum.Boss)
        {
            await RollScepter(battle.Stages.Count, 1);
            await RollMagicUnit(battle.Stages.Count, 1, [RogueMagicUnitCategoryEnum.Ultra]);
            await GainMoney(Random.Shared.Next(150, 200) * battle.Stages.Count);
        }
        else
        {
            await RollMagicUnit(battle.Stages.Count, 1, [RogueMagicUnitCategoryEnum.Common]);
            await GainMoney(Random.Shared.Next(30, 50) * battle.Stages.Count);
        }
    }

    public override async ValueTask OnPropDestruct(EntityProp prop)
    {
        await base.OnPropDestruct(prop);

        var inst = CurLevel?.CurRoom?.AdventureInstance;
        if (inst != null && inst.Excel.AdventureType == RogueAdventureGameplayTypeEnum.RogueDestroyProp)
        {
            inst.Score++;

            await Player.SendPacket(new PacketSyncRogueAdventureRoomInfoScNotify(inst));
        }
    }

    #endregion

    #region Serialization

    public RogueMagicCurInfo ToProto()
    {
        var proto = new RogueMagicCurInfo
        {
            Lineup = ToLineupInfo(),
            Level = ToLevelInfo(),
            ItemValue = ToGameItemValueInfo(),
            MiracleInfo = ToMiracleInfo(),
            GameDifficultyInfo = ToDifficultyInfo(),
            MagicItem = ToMagicItemInfo(),
            BasicInfo = ToCurAreaInfo(),
            JDMGJDBMHEJ = new LKJMDOAHGIN()
        };

        return proto;
    }

    public RogueTournCurAreaInfo ToCurAreaInfo()
    {
        var proto = new RogueTournCurAreaInfo
        {
            RogueSubMode = (uint)RogueSubMode,
            SubAreaId = (uint)AreaExcel.AreaID,
            PendingAction = RogueActions.Count > 0
                ? RogueActions.First().Value.ToProto()
                : new RogueCommonPendingAction() // to serialize empty action
        };

        return proto;
    }

    public RogueMagicGameItemInfo ToMagicItemInfo()
    {
        var proto = new RogueMagicGameItemInfo
        {
            GameStyleType = (uint)StyleType
        };

        return proto;
    }

    public RogueMagicGameDifficultyInfo ToDifficultyInfo()
    {
        var proto = new RogueMagicGameDifficultyInfo
        {
            DifficultyIdList = { DifficultyCompExcels.Select(x => (uint)x.DifficultyCompID) }
        };

        return proto;
    }

    public RogueGameItemValue ToGameItemValueInfo()
    {
        return new RogueGameItemValue
        {
            VirtualItem = { { 31, (uint)CurMoney } }
        };
    }

    public RogueTournLineupInfo ToLineupInfo()
    {
        return new RogueTournLineupInfo
        {
            AvatarIdList = { Player.LineupManager!.GetCurLineup()!.BaseAvatars!.Select(x => (uint)x.BaseAvatarId) },
            RogueReviveCost = new ItemCostData
            {
                ItemList =
                {
                    new ItemCost
                    {
                        PileItem = new PileItem
                        {
                            ItemId = 31,
                            ItemNum = (uint)CurReviveCost
                        }
                    }
                }
            }
        };
    }

    public ChessRogueMiracleInfo ToMiracleInfo()
    {
        var proto = new ChessRogueMiracleInfo
        {
            ChessRogueMiracleInfo_ = new ChessRogueMiracle()
        };

        proto.ChessRogueMiracleInfo_.MiracleList.AddRange(RogueMiracles.Select(x => x.Value.ToGameMiracleProto())
            .ToList());

        return proto;
    }


    public RogueMagicGameLevelInfo ToLevelInfo()
    {
        var proto = new RogueMagicGameLevelInfo
        {
            Status = LevelStatus,
            CurLevelIndex = (uint)(CurLevel?.CurRoomIndex ?? 0),
            Reason = RogueMagicSettleReason.None,
            ExtraRoundLimit = (uint)ExtraRoundCnt
        };

        foreach (var levelInstance in Levels.Values) proto.LevelInfoList.Add(levelInstance.ToProto());

        return proto;
    }

    public RogueMagicCurSceneInfo ToCurSceneInfo()
    {
        return new RogueMagicCurSceneInfo
        {
            Lineup = Player.LineupManager!.GetCurLineup()!.ToProto(),
            Scene = Player.SceneInstance!.ToProto(),
            //RotateInfo = new RogueMapRotateInfo
            //{
            //    IsRotate = CurLevel?.CurRoom?.Config?.RotateInfo.IsRotate ?? false,
            //    BJPBAJECKFO = (uint)(CurLevel?.CurRoom?.Config?.RotateInfo.RotateNum ?? 0)  // HDEHHKEMOCD
            //}
            RotateInfo = new RogueMapRotateInfo()
        };
    }

    #endregion
}