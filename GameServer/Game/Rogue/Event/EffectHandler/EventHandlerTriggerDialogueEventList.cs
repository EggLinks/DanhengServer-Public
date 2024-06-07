using EggLink.DanhengServer.Enums.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.TriggerDialogueEventList)]
    public class EventHandlerTriggerDialogueEventList : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            foreach (var param in ParamList)
            {
                eventInstance!.Options.Add(new()
                {
                    OptionId = param,
                });
                rogue.TriggerEvent(eventInstance, param);
            }
        }
    }
}
