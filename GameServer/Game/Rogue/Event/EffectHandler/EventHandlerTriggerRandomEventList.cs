using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.TriggerRandomEventList)]
public class EventHandlerTriggerRandomEventList : RogueEventEffectHandler
{
    public override void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option)
    {
    }

    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList, RogueEventParam? option)
    {
        var list = new RandomList<int>();
        for (var i = 0; i < paramList.Count; i += 2) list.Add(paramList[i], paramList[i + 1]);

        var randomEvent = list.GetRandom();
        eventInstance?.EffectEventId.SafeAdd(randomEvent);
        await rogue.TriggerEvent(eventInstance, randomEvent);

        await System.Threading.Tasks.Task.CompletedTask;
    }
}