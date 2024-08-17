using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.TriggerBattle)]
public class EventHandlerTriggerBattle : RogueEventEffectHandler
{
    public override void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option)
    {
    }

    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList, RogueEventParam? option)
    {
        if (option == null) return;
        if (paramList.Count == 0) return;
        option.Results.Add(new RogueEventResultInfo
        {
            BattleEventId = paramList[0]
        });
        await ValueTask.CompletedTask;
    }
}