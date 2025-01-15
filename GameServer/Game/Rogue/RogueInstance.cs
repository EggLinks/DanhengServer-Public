using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue.Buff;
using EggLink.DanhengServer.GameServer.Game.Rogue.Event;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue;

public class RogueInstance : BaseRogueInstance
{
    #region Initialization

    public RogueInstance(RogueAreaConfigExcel areaExcel, RogueAeonExcel aeonExcel, PlayerInstance player) : base(player,
        RogueSubModeEnum.CosmosRogue, aeonExcel.RogueBuffType)
    {
        AreaExcel = areaExcel;
        AeonExcel = aeonExcel;
        AeonId = aeonExcel.AeonID;
        Player = player;
        CurLineup = player.LineupManager!.GetCurLineup()!;
        EventManager = new RogueEventManager(player, this);

        foreach (var item in areaExcel.RogueMaps.Values)
        {
            RogueRooms.Add(item.SiteID, new RogueRoomInstance(item));
            if (item.IsStart) StartSiteId = item.SiteID;
        }

        // add bonus
        var action = new RogueActionInstance
        {
            QueuePosition = CurActionQueuePosition
        };
        action.SetBonus();

        RogueActions.Add(CurActionQueuePosition, action);
    }

    #endregion

    #region Properties

    public RogueStatus Status { get; set; } = RogueStatus.Doing;
    public int CurReachedRoom { get; set; }

    public RogueAeonExcel AeonExcel { get; set; }
    public RogueAreaConfigExcel AreaExcel { get; set; }
    public Dictionary<int, RogueRoomInstance> RogueRooms { get; set; } = [];
    public RogueRoomInstance? CurRoom { get; set; }
    public int StartSiteId { get; set; }

    #endregion

    #region Buffs

    public override async ValueTask RollBuff(int amount)
    {
        if (CurRoom!.Excel.RogueRoomType == 6)
        {
            await RollBuff(amount, 100003, 2); // boss room
            await RollMiracle(1);
        }
        else
        {
            await RollBuff(amount, 100005);
        }
    }

    public async ValueTask AddAeonBuff()
    {
        if (AeonBuffPending) return;
        if (CurAeonBuffCount + CurAeonEnhanceCount >= 4)
            // max aeon buff count
            return;
        var curAeonBuffCount = 0; // current path buff count
        var hintId = AeonId * 100 + 1;
        var enhanceData = GameData.RogueAeonEnhanceData[AeonId];
        var buffData = GameData.RogueAeonBuffData[AeonId];
        foreach (var buff in RogueBuffs)
            if (buff.BuffExcel.RogueBuffType == AeonExcel.RogueBuffType)
            {
                if (!(buff.BuffExcel as RogueBuffExcel)!.IsAeonBuff)
                {
                    curAeonBuffCount++;
                }
                else
                {
                    hintId++; // next hint
                    enhanceData.Remove((buff.BuffExcel as RogueBuffExcel)!);
                }
            }

        var needAeonBuffCount = (CurAeonBuffCount + CurAeonEnhanceCount) switch
        {
            0 => 3,
            1 => 6,
            2 => 10,
            3 => 14,
            _ => 100
        };

        if (curAeonBuffCount >= needAeonBuffCount)
        {
            RogueBuffSelectMenu menu = new(this)
            {
                QueueAppend = 2,
                HintId = hintId,
                RollMaxCount = 0,
                RollFreeCount = 0,
                IsAeonBuff = true
            };
            if (CurAeonBuffCount < 1)
            {
                CurAeonBuffCount++;
                // add aeon buff
                menu.RollBuff([buffData], 1);
            }
            else
            {
                CurAeonEnhanceCount++;
                // add enhance buff
                menu.RollBuff(enhanceData.Select(x => x as BaseRogueBuffExcel).ToList(), enhanceData.Count);
            }

            var action = menu.GetActionInstance();
            RogueActions.Add(action.QueuePosition, action);

            AeonBuffPending = true;
            await UpdateMenu();
        }
    }

    #endregion

    #region Methods

    public override async ValueTask UpdateMenu(int position = 0)
    {
        await base.UpdateMenu(position);


        await AddAeonBuff(); // check if aeon buff can be added
    }

    public async ValueTask<RogueRoomInstance?> EnterRoom(int siteId)
    {
        var prevRoom = CurRoom;
        if (prevRoom != null)
        {
            if (!prevRoom.NextSiteIds.Contains(siteId)) return null;
            prevRoom.Status = RogueRoomStatus.Finish;
            // send
            await Player.SendPacket(new PacketSyncRogueMapRoomScNotify(prevRoom, AreaExcel.MapId));
        }

        // next room
        CurReachedRoom++;
        CurRoom = RogueRooms[siteId];
        CurRoom.Status = RogueRoomStatus.Play;

        await Player.EnterScene(CurRoom.Excel.MapEntrance, 0, false);

        // move
        var anchor = Player.SceneInstance!.FloorInfo?.GetAnchorInfo(CurRoom.Excel.GroupID, 1);
        if (anchor != null)
        {
            Player.Data.Pos = anchor.ToPositionProto();
            Player.Data.Rot = anchor.ToRotationProto();
        }

        // send
        await Player.SendPacket(new PacketSyncRogueMapRoomScNotify(CurRoom, AreaExcel.MapId));

        // call event
        EventManager?.OnNextRoom();
        foreach (var miracle in RogueMiracles.Values) miracle.OnEnterNextRoom();

        return CurRoom;
    }

    public async ValueTask LeaveRogue()
    {
        Player.RogueManager!.RogueInstance = null;
        await Player.EnterScene(801120102, 0, false);
        Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);
    }

    public async ValueTask QuitRogue()
    {
        Status = RogueStatus.Finish;
        await Player.SendPacket(new PacketSyncRogueStatusScNotify(Status));
        await Player.SendPacket(new PacketSyncRogueFinishScNotify(ToFinishInfo()));
    }

    #endregion

    #region Handlers

    public override void OnBattleStart(BattleInstance battle)
    {
        base.OnBattleStart(battle);

        GameData.RogueMapData.TryGetValue(AreaExcel.MapId, out var mapData);
        if (mapData != null)
        {
            mapData.TryGetValue(CurRoom!.SiteId, out var mapInfo);
            if (mapInfo != null && mapInfo.LevelList.Count > 0) battle.CustomLevel = mapInfo.LevelList.RandomElement();
        }
    }

    public override async ValueTask OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req)
    {
        foreach (var miracle in RogueMiracles.Values) miracle.OnEndBattle(battle);

        if (req.EndStatus != BattleEndStatus.BattleEndWin)
        {
            // quit
            await QuitRogue();
            return;
        }

        if (CurRoom!.NextSiteIds.Count == 0)
        {
            // last room
            IsWin = true;
            await Player.SendPacket(new PacketSyncRogueExploreWinScNotify());
        }
        else
        {
            await RollBuff(battle.Stages.Count);
            await GainMoney(Random.Shared.Next(20, 60) * battle.Stages.Count);
        }
    }

    #endregion

    #region Serialization

    public RogueCurrentInfo ToProto()
    {
        var proto = new RogueCurrentInfo
        {
            Status = Status,
            GameMiracleInfo = ToMiracleInfo(),
            RogueAeonInfo = ToAeonInfo(),
            RogueLineupInfo = ToLineupInfo(),
            RogueBuffInfo = ToBuffInfo(),
            VirtualItemInfo = ToVirtualItemInfo(),
            RogueMap = ToMapInfo(),
            ModuleInfo = new RogueModuleInfo
            {
                ModuleIdList = { 1, 2, 3, 4, 5 }
            },
            IsExploreWin = IsWin
        };

        if (RogueActions.Count > 0)
            proto.PendingAction = RogueActions.First().Value.ToProto();
        else
            proto.PendingAction = new RogueCommonPendingAction();

        return proto;
    }

    public GameAeonInfo ToAeonInfo()
    {
        return new GameAeonInfo
        {
            GameAeonId = (uint)AeonId,
            IsUnlocked = AeonId != 0,
            UnlockedAeonEnhanceNum = (uint)(AeonId != 0 ? 3 : 0)
        };
    }

    public RogueLineupInfo ToLineupInfo()
    {
        var proto = new RogueLineupInfo();

        foreach (var avatar in CurLineup!.BaseAvatars!) proto.BaseAvatarIdList.Add((uint)avatar.BaseAvatarId);

        proto.ReviveInfo = new RogueReviveInfo
        {
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

        return proto;
    }

    public RogueVirtualItem ToVirtualItemInfo()
    {
        return new RogueVirtualItem
        {
            RogueMoney = (uint)CurMoney
        };
    }

    public RogueMapInfo ToMapInfo()
    {
        var proto = new RogueMapInfo
        {
            CurSiteId = (uint)CurRoom!.SiteId,
            CurRoomId = (uint)CurRoom!.RoomId,
            AreaId = (uint)AreaExcel.RogueAreaID,
            MapId = (uint)AreaExcel.MapId
        };

        foreach (var room in RogueRooms) proto.RoomList.Add(room.Value.ToProto());

        return proto;
    }

    public GameMiracleInfo ToMiracleInfo()
    {
        var proto = new GameMiracleInfo
        {
            GameMiracleInfo_ = new RogueMiracleInfo()
        };

        foreach (var miracle in RogueMiracles.Values) proto.GameMiracleInfo_.MiracleList.Add(miracle.ToProto());

        return proto;
    }

    public RogueBuffInfo ToBuffInfo()
    {
        var proto = new RogueBuffInfo();

        foreach (var buff in RogueBuffs) proto.MazeBuffList.Add(buff.ToProto());

        return proto;
    }

    public RogueFinishInfo ToFinishInfo()
    {
        AreaExcel.ScoreMap.TryGetValue(CurReachedRoom, out var score);
        var prev = Player.RogueManager!.ToRewardProto();
        Player.RogueManager!.AddRogueScore(score);
        var next = Player.RogueManager!.ToRewardProto();

        return new RogueFinishInfo
        {
            ScoreId = (uint)score,
            //TotalScore = (uint)score,
            //PrevRewardInfo = prev,
            //NextRewardInfo = next,
            AreaId = (uint)AreaExcel.RogueAreaID,
            //FinishedRoomCount = (uint)CurReachedRoom,
            //ReachedRoomCount = (uint)CurReachedRoom,
            IsWin = IsWin,
            RecordInfo = new RogueRecordInfo
            {
                AvatarList =
                {
                    CurLineup!.BaseAvatars!.Select(avatar => new RogueRecordAvatar
                    {
                        Id = (uint)avatar.BaseAvatarId,
                        AvatarType = AvatarType.AvatarFormalType,
                        Level = (uint)(Player.AvatarManager!.GetAvatar(avatar.BaseAvatarId)?.Level ?? 0),
                        Slot = (uint)CurLineup!.BaseAvatars!.IndexOf(avatar)
                    })
                },
                BuffList = { RogueBuffs.Select(buff => buff.ToProto()) },
                MiracleList = { RogueMiracles.Values.Select(miracle => (uint)miracle.MiracleId) }
            }
        };
    }

    #endregion
}