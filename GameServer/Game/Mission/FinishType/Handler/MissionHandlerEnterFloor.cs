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
    [MissionFinishType(MissionFinishTypeEnum.EnterFloor)]
    public class MissionHandlerEnterFloor : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            player.EnterMissionScene(info.MapEntranceID, info.AnchorGroupID, info.AnchorID, true);
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (player.Data.FloorId == info.ParamInt2)
            {
                player.MissionManager!.FinishSubMission(info.ID);
            }
        }
    }
}
