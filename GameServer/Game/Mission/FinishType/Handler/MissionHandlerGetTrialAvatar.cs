using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.GetTrialAvatar)]
    public class MissionHandlerGetTrialAvatar : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (player.LineupManager!.GetCurLineup() == null) return;
            var actualSpecialAvatarId = info.ParamInt1 * 10 + player.Data.WorldLevel;
            var item = player.LineupManager!.GetCurLineup()!.BaseAvatars!.Find(item => item.SpecialAvatarId == actualSpecialAvatarId);
            if (item != null) return;  // existing avatar
            player.LineupManager!.AddSpecialAvatarToCurTeam(actualSpecialAvatarId);
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            player.MissionManager!.FinishSubMission(info.ID);
        }
    }
}
