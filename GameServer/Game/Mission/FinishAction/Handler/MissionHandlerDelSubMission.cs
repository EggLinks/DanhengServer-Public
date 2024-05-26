using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Mission.FinishAction.Handler
{
    [MissionFinishAction(FinishActionTypeEnum.delSubMission)]
    public class MissionHandlerDelSubMission : MissionFinishActionHandler
    {
        public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
        {
            if (Params.Count < 1) return;
            var subMissionId = Params[0];
            Player.MissionManager?.AcceptSubMission(subMissionId);
            Player.MissionManager?.FinishSubMission(subMissionId);
        }
    }
}
