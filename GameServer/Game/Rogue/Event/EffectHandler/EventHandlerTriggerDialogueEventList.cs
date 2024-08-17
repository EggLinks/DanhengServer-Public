using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.TriggerDialogueEventList)]
public class EventHandlerTriggerDialogueEventList : RogueEventEffectHandler
{
    public override void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option)
    {
    }

    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList, RogueEventParam? option)
    {
        foreach (var param in paramList)
        {
            eventInstance?.EffectEventId.SafeAdd(param);
            await rogue.TriggerEvent(eventInstance, param);
        }

        await System.Threading.Tasks.Task.CompletedTask;
    }
}