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
    [MissionFinishType(MissionFinishTypeEnum.MessagePerformSectionFinish)]
    public class MissionHandlerMessagePerformSectionFinish : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            player.MessageManager!.AddMessageSection(info.ParamInt1);
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            var data = player.MessageManager!.GetMessageSectionData(info.ParamInt1);
            if (data == null)
            {
                return;
            }

            if (data.Status == Proto.MessageSectionStatus.MessageSectionFinish)
            {
                player.MissionManager!.FinishSubMission(info.ID);
            }
        }
    }
}
