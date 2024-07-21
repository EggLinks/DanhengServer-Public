using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Game.Mission.FinishType;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(Enums.MissionFinishTypeEnum.FloorSavedValue)]
    public class MissionHandlerFloorSavedValue : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (player.SceneData?.FloorSavedData.TryGetValue(info.LevelFloorID, out var floor) == true)
            {
                if (floor.TryGetValue(info.ParamStr1, out var value))
                {
                    if (value == info.ParamInt1)
                    {
                        player.MissionManager?.FinishSubMission(info.ID);
                    }
                }
            }
        }
    }
}
