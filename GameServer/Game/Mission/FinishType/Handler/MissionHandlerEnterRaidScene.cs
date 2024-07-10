using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.EnterRaidScene)]
    public class MissionHandlerEnterRaidScene : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (player.RaidManager!.RaidData.CurRaidId != info.ParamInt1)
            {
                // change raid
                player.RaidManager!.EnterRaid(info.ParamInt1, 0);
            }
            player.EnterScene(info.ParamInt2, 0, true);
            player.MissionManager!.FinishSubMission(info.ID);
        }
    }
}
