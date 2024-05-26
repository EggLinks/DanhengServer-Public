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
    [MissionFinishType(MissionFinishTypeEnum.FinishMission)]
    public class MissionHandlerFinishMission : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            var send = true;
            foreach (var mainMissionId in info.ParamIntList)
            {
                if (player.MissionManager!.GetMainMissionStatus(mainMissionId) != MissionPhaseEnum.Finish)
                {
                    send = false;
                    break;
                }
            }
            if (send)
            {
                player.MissionManager!.FinishSubMission(info.ID);
            }
        }
    }
}
