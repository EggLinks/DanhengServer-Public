using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Mission.FinishType;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.HeartDialDialoguePerformanceFinish)]
    public class MissionHandlerHeartDialDialoguePerformanceFinish : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (arg is string str && str.StartsWith("HeartDial_"))
            {
                var dialogueId = int.Parse(str.Replace("HeartDial_", ""));
                if (info.ParamIntList?.Contains(dialogueId) == true)
                {
                    player.MissionManager!.AddMissionProgress(info.ID, 1);
                    var curProgress = player.MissionManager!.GetMissionProgress(info.ID);
                    if (curProgress >= info.Progress)  // finish count >= progress, finish mission
                    {
                        player.MissionManager!.FinishSubMission(info.ID);
                    }
                }
            }
        }
    }
}
