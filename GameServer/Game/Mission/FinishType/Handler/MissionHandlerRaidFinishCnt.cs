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
    [MissionFinishType(MissionFinishTypeEnum.RaidFinishCnt)]
    public class MissionHandlerRaidFinishCnt : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            var finishCount = 0;
            foreach (var raidId in info.ParamIntList ?? [])
            {
                if (player.RaidManager!.GetRaidStatus(raidId) == Proto.RaidStatus.Finish)
                {
                    finishCount++;
                }
            }

            if (finishCount >= info.Progress)
            {
                player.MissionManager!.FinishSubMission(info.ID);
            }
        }
    }
}
