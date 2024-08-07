using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.TriggerRogueMiracleSelect)]
public class EventHandlerTriggerRogueMiracleSelect : RogueEventEffectHandler
{
    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList)
    {
        await rogue.RollMiracle(paramList[2], paramList[0]);
    }
}