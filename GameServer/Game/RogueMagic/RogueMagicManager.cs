using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic;

public class RogueMagicManager(PlayerInstance player) : BasePlayerManager(player)
{
    public RogueMagicInstance? RogueMagicInstance { get; set; }


    public async ValueTask<(Retcode, RogueMagicInstance?)> StartRogueMagic(List<int> avatars, int area, int styleType,
        List<int> difficulty)
    {
        RogueMagicInstance = null;
        var areaExcel = GameData.RogueMagicAreaData.GetValueOrDefault(area);

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

        Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupMagicRogue, baseAvatarIds);
        await Player.LineupManager!.GainMp(5, false);
        await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));

        var instance = new RogueMagicInstance(Player, area, difficulty, styleType);
        RogueMagicInstance = instance;
        await instance.EnterRoom(1, RogueMagicRoomTypeEnum.Battle);
        return (Retcode.RetSucc, instance);
    }

    #region Serialization

    public RogueMagicGetInfo ToGetInfo()
    {
        var proto = new RogueMagicGetInfo
        {
            StoryInfo = ToStoryInfo(),
            RogueMagicTalentInfo = ToTalentInfo()
        };

        proto.RogueTournAreaInfo.AddRange(ToAreaInfoList());
        proto.RogueTournDifficultyInfo.AddRange(ToDifficultyInfoList());

        return proto;
    }

    public RogueMagicStoryInfo ToStoryInfo()
    {
        var proto = new RogueMagicStoryInfo
        {
            FinishedMagicStoryList = { GameData.RogueMagicStoryData.Keys.Select(x => (uint)x) }
        };

        return proto;
    }

    public RogueMagicTalentInfo ToTalentInfo()
    {
        var proto = new RogueMagicTalentInfo
        {
            TalentInfoList = new RogueTalentInfoList
            {
                TalentInfo =
                {
                    GameData.RogueMagicTalentData.Keys.Select(x => new RogueTalentInfo
                    {
                        TalentId = (uint)x,
                        Status = RogueTalentStatus.Enable
                    })
                }
            }
        };

        return proto;
    }

    public List<RogueMagicAreaInfo> ToAreaInfoList()
    {
        return (from area in GameData.RogueMagicAreaData.Values
            select new RogueMagicAreaInfo
                { AreaId = (uint)area.AreaID, Completed = true, IsUnlocked = true, IsTakenReward = true }).ToList();
    }

    public List<RogueMagicDifficultyInfo> ToDifficultyInfoList()
    {
        return (from difficulty in GameData.RogueMagicDifficultyCompData.Values
            select new RogueMagicDifficultyInfo
                { DifficultyId = (uint)difficulty.DifficultyCompID, IsUnlocked = true }).ToList();
    }

    #endregion
}