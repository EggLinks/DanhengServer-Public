using EggLink.DanhengServer.Enums.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.TriggerRogueMiracleSelect)]
    public class EventHandlerTriggerRogueMiracleSelect : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            rogue.RollMiracle(ParamList[2], ParamList[0]);
        }
    }
}
