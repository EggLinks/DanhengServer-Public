using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Game.Mission.FinishType;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(Enums.MissionFinishTypeEnum.HeartDialScriptListStep)]
    public class MissionHandlerHeartDialScriptListStep : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            var count = 0;
            foreach (var scriptId in info.ParamIntList ?? [])
            {
                HeartDialInfo? dialInfo = null;
                player.HeartDialData?.DialList.TryGetValue(scriptId, out dialInfo);
                if (dialInfo != null && (int)dialInfo.StepType == info.ParamInt1)
                {
                    count++;
                }
            }

            if (count >= info.Progress)
            {
                player.MissionManager?.FinishSubMission(info.ID);
            }
            else
            {
                if (player.MissionManager?.GetMissionProgress(info.ID) != count)
                    player.MissionManager?.SetMissionProgress(info.ID, count);
            }
        }
    }
}
