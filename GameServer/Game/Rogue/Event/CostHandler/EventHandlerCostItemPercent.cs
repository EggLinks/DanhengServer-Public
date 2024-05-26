using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.CostHandler
{
    [RogueEvent(costType: DialogueEventCostTypeEnum.CostItemPercent)]
    public class EventHandlerCostItemPercent : RogueEventCostHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            rogue.CostMoney((int)(rogue.CurMoney * (ParamList[1] / 100f)), RogueActionDisplayType.RogueCommonActionResultDisplayTypeSingle);
        }
    }
}
