using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Mission.FinishAction;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction.Handler
{
    [MissionFinishAction(FinishActionTypeEnum.EnterEntryIfNotThere)]
    public class MissionHandlerEnterEntryIfNotThere : MissionFinishActionHandler
    {
        public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
        {
            var entryId = Params[0];
            var anchorGroup = Params[1];
            var anchorId = Params[2];

            Player.EnterMissionScene(entryId, anchorGroup, anchorId, true);
        }
    }
}
