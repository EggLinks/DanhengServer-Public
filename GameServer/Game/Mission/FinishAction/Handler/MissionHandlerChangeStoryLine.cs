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
    [MissionFinishAction(FinishActionTypeEnum.ChangeStoryLine)]
    public class MissionHandlerChangeStoryLine : MissionFinishActionHandler
    {
        public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
        {
            var toStoryLineId = Params[0];
            var toEntryId = Params[1];
            var toAnchorGroup = Params[2];
            var toAnchorId = Params[3];

            if (toStoryLineId == 0)
            {
                // exit
                Player.StoryLineManager!.FinishStoryLine(toEntryId, toAnchorGroup, toAnchorId);
            } 
            else
            {
                Player.StoryLineManager!.InitStoryLine(toStoryLineId, toEntryId, toAnchorGroup, toAnchorId);
            }
        }
    }
}
