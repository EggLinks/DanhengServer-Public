using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.RogueTourn;

public class RogueTournManager(PlayerInstance player) : BasePlayerManager(player)
{
    public RogueTournInstance? RogueTournInstance { get; set; }

    public async ValueTask<(Retcode, RogueTournInstance?)> StartRogueTourn(List<int> avatars, int area, int week,
        List<int> difficulty)
    {
        RogueTournInstance = null;
        var areaExcel = GameData.RogueTournAreaData.GetValueOrDefault(area);

        if (areaExcel == null)
            return (Retcode.RetRogueAreaInvalid, null);

        var baseAvatarIds = new List<int>();
        foreach (var avatar in avatars.Select(id => Player.AvatarManager!.GetAvatar(id)))
        {
            if (avatar == null)
                return (Retcode.RetAvatarNotExist, null);

            avatar.SetCurHp(10000, true);
            avatar.SetCurSp(5000, true);
            baseAvatarIds.Add(avatar.GetBaseAvatarId());
        }

        Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupTournRogue, baseAvatarIds);
        await Player.LineupManager!.GainMp(5, false);
        await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));

        var instance = new RogueTournInstance(Player, area);
        RogueTournInstance = instance;
        await instance.EnterRoom(1, RogueTournRoomTypeEnum.Battle);
        return (Retcode.RetSucc, instance);
    }

    #region Serialization

    public RogueTournInfo ToProto()
    {
        var proto = new RogueTournInfo
        {
            ExtraScoreInfo = ToExtraScoreProto(),
            PermanentInfo = ToPermanentTalentProto(),
            RogueSeasonInfo = ToSeasonProto(),
            RogueTournAreaInfo = { ToAreaProtoList() },
            RogueTournDifficultyInfo = { ToDifficultyProtoList() },
            RogueTournExpInfo = ToExpProto(),
            RogueTournHandbook = ToHandbookProto(),
            RogueTournSaveList =
            {
                Capacity = 0
            }
        };

        return proto;
    }

    public ExtraScoreInfo ToExtraScoreProto()
    {
        return new ExtraScoreInfo
        {
            EndTime = RogueManager.GetCurrentRogueTime().Item2,
            Week = 1
        };
    }

    public RogueTournPermanentTalentInfo ToPermanentTalentProto()
    {
        return new RogueTournPermanentTalentInfo
        {
            TalentInfoList = new RogueTalentInfoList
            {
                TalentInfo =
                {
                    GameData.RogueTournPermanentTalentData.Values.Select(x => new RogueTalentInfo
                    {
                        TalentId = (uint)x.TalentID,
                        Status = RogueTalentStatus.Enable
                    })
                }
            }
        };
    }

    public RogueTournSeasonInfo ToSeasonProto()
    {
        return new RogueTournSeasonInfo
        {
            SubTournId = 1,
            MainTournId = 1
        };
    }

    public List<RogueTournAreaInfo> ToAreaProtoList()
    {
        return (from areaExcel in GameData.RogueTournAreaData
                where areaExcel.Value.AreaGroupID != RogueTournAreaGroupIDEnum.WeekChallenge
                select new RogueTournAreaInfo
                {
                    AreaId = (uint)areaExcel.Value.AreaID, Completed = true, IsTakenReward = true, IsUnlocked = true
                })
            .ToList();
    }

    public List<RogueTournDifficultyInfo> ToDifficultyProtoList()
    {
        return (from difficultyExcel in GameData.RogueTournDifficultyCompData.Values
            select new RogueTournDifficultyInfo
                { DifficultyId = (uint)difficultyExcel.DifficultyCompID, IsUnlocked = true }).ToList();
    }

    public RogueTournExpInfo ToExpProto()
    {
        return new RogueTournExpInfo
        {
            Exp = 0,
            TakenLevelRewards =
            {
                Capacity = 0
            }
        };
    }

    public RogueTournHandbookInfo ToHandbookProto()
    {
        var proto = new RogueTournHandbookInfo
        {
            RogueTournHandbookConst = 1
        };

        //foreach (var hexAvatar in GameData.RogueTournHexAvatarBaseTypeData.Keys)
        //    proto.HandbookAvatarBaseList.Add((uint)hexAvatar);

        //foreach (var buff in GameData.RogueBuffData.Values)
        //    if (buff is RogueTournBuffExcel { IsInHandbook: true })
        //        proto.HandbookBuffList.Add((uint)buff.MazeBuffID);

        //foreach (var formulaId in GameData.RogueTournFormulaData.Keys) proto.HandbookFormulaList.Add((uint)formulaId);

        //foreach (var miracleId in GameData.RogueTournHandbookMiracleData.Keys)
        //    proto.HandbookMiracleList.Add((uint)miracleId);

        //foreach (var eventId in GameData.RogueTournHandBookEventData.Keys) proto.HandbookEventList.Add((uint)eventId);

        return proto;
    }

    #endregion
}