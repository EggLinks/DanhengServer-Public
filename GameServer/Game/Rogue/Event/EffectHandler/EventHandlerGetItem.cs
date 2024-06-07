using EggLink.DanhengServer.Enums.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.GetItem)]
    public class EventHandlerGetItem : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            rogue.GainMoney(ParamList[1], ParamList[2], Proto.RogueActionDisplayType.RogueCommonActionResultDisplayTypeSingle);
        }
    }
}
