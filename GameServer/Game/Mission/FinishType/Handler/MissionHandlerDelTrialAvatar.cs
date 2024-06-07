using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.DelTrialAvatar)]
    public class MissionHandlerDelTrialAvatar : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (player.LineupManager!.GetCurLineup() == null) return;
            var actualSpecialAvatarId = info.ParamInt1 * 10 + player.Data.WorldLevel;
            var item = player.LineupManager!.GetCurLineup()!.BaseAvatars!.Find(item => item.SpecialAvatarId == actualSpecialAvatarId);
            if (item == null) return;  // avatar not found
            player.LineupManager!.RemoveSpecialAvatarFromCurTeam(actualSpecialAvatarId);
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            player.MissionManager!.FinishSubMission(info.ID);
        }
    }
}
