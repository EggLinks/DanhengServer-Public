using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue;

public class ChessRogueManager(PlayerInstance player) : BasePlayerManager(player)
{
    public ChessRogueNousData ChessRogueNousData { get; } =
        DatabaseHelper.Instance!.GetInstanceOrCreateNew<ChessRogueNousData>(player.Uid);

    public ChessRogueInstance? RogueInstance { get; set; }

    #region Game Management

    public async ValueTask StartRogue(int aeonId, List<uint> avatarIds, int areaId, int branchId,
        List<int> difficultyIds, List<int> disableAeonIdList)
    {
        GameData.RogueNousAeonData.TryGetValue(aeonId, out var aeonData);
        GameData.RogueDLCAreaData.TryGetValue(areaId, out var areaData);
        if (aeonData == null || areaData == null) return;

        Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupChessRogue, avatarIds.Select(x => (int)x).ToList());
        await Player.LineupManager!.GainMp(5, false);
        await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));

        foreach (var id in avatarIds)
        {
            Player.AvatarManager!.GetAvatar((int)id)?.SetCurHp(10000, true);
            Player.AvatarManager!.GetAvatar((int)id)?.SetCurSp(10000, true);
        }

        var difficultyLevel = difficultyIds.Select(x => GameData.RogueNousDifficultyLevelData[x]).ToList();

        var instance = new ChessRogueInstance(Player, areaData, aeonData, areaData.RogueVersionId, branchId)
        {
            DisableAeonIds = disableAeonIdList,
            DifficultyLevel = difficultyLevel
        };

        RogueInstance = instance;

        await instance.EnterCell(instance.StartCell);

        await Player.SendPacket(new PacketChessRogueStartScRsp(Player));
    }

    #endregion

    #region Dice Management

    public ChessRogueNousDiceData GetDice(int branchId)
    {
        ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);

        if (diceData == null) // set to default
        {
            var branch = GameData.RogueNousDiceBranchData[branchId];
            var surface = branch.GetDefaultSurfaceList();
            return SetDice(branchId,
                surface.Select((id, i) => new { id, i })
                    .ToDictionary(x => x.i + 1, x => x.id)); // convert to dictionary
        }

        return diceData;
    }

    public ChessRogueNousDiceData SetDice(int branchId, Dictionary<int, int> surfaceId)
    {
        ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);

        if (diceData == null)
        {
            diceData = new ChessRogueNousDiceData
            {
                BranchId = branchId,
                Surfaces = surfaceId
            };

            ChessRogueNousData.RogueDiceData[branchId] = diceData;
        }
        else
        {
            diceData.Surfaces = surfaceId;
        }

        return diceData;
    }

    public ChessRogueNousDiceData SetDice(int branchId, int index, int surfaceId)
    {
        ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);
        if (diceData == null)
        {
            // set to default
            var branch = GameData.RogueNousDiceBranchData[branchId];
            var surface = branch.GetDefaultSurfaceList();
            surface[index] = surfaceId;

            return SetDice(branchId,
                surface.Select((id, i) => new { id, i })
                    .ToDictionary(x => x.i + 1, x => x.id)); // convert to dictionary
        }

        diceData.Surfaces[index] = surfaceId;

        return diceData;
    }

    public ChessRogueNousDiceData SetDice(ChessRogueDice dice)
    {
        var branchId = (int)dice.DiceBranchId;
        ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);
        if (diceData == null)
        {
            // set to default
            var branch = GameData.RogueNousDiceBranchData[branchId];
            var surface = branch.GetDefaultSurfaceList();

            foreach (var d in dice.SurfaceList) surface[(int)d.Index - 1] = (int)d.SurfaceId;

            return SetDice(branchId,
                surface.Select((id, i) => new { id, i })
                    .ToDictionary(x => x.i + 1, x => x.id)); // convert to dictionary
        }

        foreach (var d in dice.SurfaceList) diceData.Surfaces[(int)d.Index] = (int)d.SurfaceId;

        return diceData;
    }

    #endregion

    #region Serialization

    public ChessRogueGetInfo ToGetInfo()
    {
        var info = new ChessRogueGetInfo
        {
            ChessAeonInfo = ToAeonInfo(),
            QueryDiceInfo = ToDiceInfo(),
            TalentInfoList = ToTalentInfo(),
            RogueDifficultyInfo = new ChessRogueQueryDiffcultyInfo()
        };

        foreach (var area in GameData.RogueDLCAreaData.Keys)
        {
            info.AreaIdList.Add((uint)area);
            info.ExploredAreaIdList.Add((uint)area);
        }

        foreach (var item in GameData.RogueNousDifficultyLevelData.Keys)
            info.RogueDifficultyInfo.QueryDifficultyIdList.Add((uint)item);

        return info;
    }

    public ChessRogueCurrentInfo ToCurrentInfo()
    {
        if (RogueInstance != null) return RogueInstance.ToProto();
        var info = new ChessRogueCurrentInfo
        {
            RogueSubMode = 201,
            LevelInfo = ToLevelInfo(),
            RogueAeonInfo = ToRogueAeonInfo(),
            RogueDiceInfo = ToRogueDiceInfo(),
            RogueDifficultyInfo = new ChessRogueCurrentDifficultyInfo(),
            GameMiracleInfo = new ChessRogueMiracleInfo
            {
                ChessRogueMiracleInfo_ = new ChessRogueMiracle()
            }, // needed for avoiding null reference exception （below 4 lines）
            RogueBuffInfo = new ChessRogueBuffInfo { ChessRogueBuffInfo_ = new ChessRogueBuff() },
            PendingAction = new RogueCommonPendingAction(),
            RogueLineupInfo = ToLineupInfo(),
            VirtualItemInfo = new RogueVirtualItem()
        };

        return info;
    }

    public ChessRogueQueryInfo ToQueryInfo()
    {
        var info = new ChessRogueQueryInfo
        {
            ChessAeonInfo = ToAeonInfo(),
            TalentInfoList = ToTalentInfo(),
            RogueDifficultyInfo = new ChessRogueQueryDiffcultyInfo(),
            QueryDiceInfo = ToDiceInfo()
        };

        foreach (var area in GameData.RogueDLCAreaData.Keys)
        {
            info.AreaIdList.Add((uint)area);
            info.ExploredAreaIdList.Add((uint)area);
        }

        foreach (var item in GameData.RogueNousDifficultyLevelData.Keys)
            info.RogueDifficultyInfo.QueryDifficultyIdList.Add((uint)item);

        return info;
    }

    public ChessRogueLevelInfo ToLevelInfo()
    {
        var proto = new ChessRogueLevelInfo
        {
            AreaInfo = new ChessRogueAreaInfo
            {
                Cell = new CellInfo(),
                OJNCMJDAABJ = new JDIPIHPMEKN()
            }
        };

        foreach (var area in GameData.RogueDLCAreaData.Keys) proto.ExploredAreaIdList.Add((uint)area);


        return proto;
    }

    public ChessRogueQueryAeonInfo ToAeonInfo()
    {
        var proto = new ChessRogueQueryAeonInfo();

        foreach (var aeon in GameData.RogueNousAeonData.Values)
        {
            if (aeon.AeonID > 7) continue;
            proto.AeonList.Add(new ChessRogueQueryAeon
            {
                AeonId = (uint)aeon.AeonID
            });
        }

        return proto;
    }

    public ChessRogueAeonInfo ToRogueAeonInfo()
    {
        var proto = new ChessRogueAeonInfo
        {
            ChessAeonInfo = ToAeonInfo()
        };


        foreach (var aeon in GameData.RogueNousAeonData.Values)
        {
            if (aeon.AeonID > 8) continue;
            proto.AeonIdList.Add((uint)aeon.AeonID);
        }

        return proto;
    }

    public ChessRogueQueryDiceInfo ToDiceInfo()
    {
        var proto = new ChessRogueQueryDiceInfo
        {
            DicePhase = ChessRogueNousDicePhase.PhaseTwo
        };

        foreach (var branch in GameData.RogueNousDiceSurfaceData.Keys) proto.SurfaceIdList.Add((uint)branch);

        foreach (var dice in GameData.RogueNousDiceBranchData) proto.DiceList.Add(GetDice(dice.Key).ToProto());

        for (var i = 1; i < 7; i++) proto.MLKDHOECNFL.Add((uint)i, i % 3 == 0);
        proto.MLKDHOECNFL[5] = true;

        return proto;
    }

    public ChessRogueDiceInfo ToRogueDiceInfo()
    {
        var proto = new ChessRogueDiceInfo();

        return proto;
    }

    public ChessRogueTalentInfo ToTalentInfo()
    {
        var talentInfo = new RogueTalentInfoList();

        foreach (var talent in GameData.RogueNousTalentData.Values)
            talentInfo.TalentInfo.Add(new RogueTalentInfo
            {
                TalentId = (uint)talent.TalentID,
                Status = RogueTalentStatus.Enable
            });

        var proto = new ChessRogueTalentInfo
        {
            RogueTalentInfoList = talentInfo
        };

        return proto;
    }

    public ChessRogueLineupInfo ToLineupInfo()
    {
        var proto = new ChessRogueLineupInfo
        {
            ReviveInfo = new RogueAvatarReviveCost
            {
                RogueReviveCost = new ItemCostData()
            }
        };

        return proto;
    }

    #endregion
}