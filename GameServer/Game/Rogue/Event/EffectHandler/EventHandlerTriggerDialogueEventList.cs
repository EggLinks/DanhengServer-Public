using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.TriggerDialogueEventList)]
public class EventHandlerTriggerDialogueEventList : RogueEventEffectHandler
{
    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList)
    {
        foreach (var param in paramList)
        {
            eventInstance!.Options.Add(new RogueEventParam
            {
                OptionId = param
            });
            rogue.TriggerEvent(eventInstance, param);
        }

        await System.Threading.Tasks.Task.CompletedTask;
    }
}