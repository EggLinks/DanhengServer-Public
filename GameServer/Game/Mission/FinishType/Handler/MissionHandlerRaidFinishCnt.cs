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
            if (arg != null && arg is int i)
            {
                foreach (var raidId in info.ParamIntList ?? [])
                {
                    if (raidId == i)
                    {
                        player.MissionManager!.FinishSubMission(info.ID);
                        break;
                    }
                }
            }
        }
    }
}
