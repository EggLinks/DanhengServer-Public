using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.TriggerRandomEventList)]
    public class EventHandlerTriggerRandomEventList : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            var list = new RandomList<int>();
            for (int i = 0; i < ParamList.Count; i += 2)
            {
                list.Add(ParamList[i], ParamList[i + 1]);
            }
            var randomEvent = list.GetRandom();
            eventInstance!.Options.Add(new()
            {
                OptionId = randomEvent,
            });
            rogue.TriggerEvent(eventInstance, randomEvent);
        }
    }
}
