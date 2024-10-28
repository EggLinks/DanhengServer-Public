using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.TriggerRogueBuffSelect)]
public class EventHandlerTriggerRogueBuffSelect : RogueEventEffectHandler
{
    public override void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option)
    {
    }

    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList, RogueEventParam? option)
    {
        await rogue.RollBuff(paramList[2], paramList[0], paramList[1]);
    }
}