using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.RepeatableGamble)]
public class EventHandlerRepeatableGamble : RogueEventEffectHandler
{
    public override void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option)
    {
        if (option == null) return;
        option.Ratio = paramList[1] / 100f;
    }

    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList, RogueEventParam? option)
    {
        if (option == null) return;
        if (eventInstance == null) return;
        var winRatio = option.Ratio * 10000;
        var randomInt = Extensions.RandomInt(0, 10001);
        if (randomInt <= winRatio)
        {
            // win
            eventInstance.EffectEventId.SafeAdd(paramList[0]);
            option.OverrideSelected = true;
            await rogue.TriggerEvent(eventInstance, paramList[0]);
        }
        else
        {
            // lose
            eventInstance.EffectEventId.SafeAdd(paramList[3]);
            option.OverrideSelected = false;
            option.Ratio += paramList[2] / 100f;
            await rogue.TriggerEvent(eventInstance, paramList[3]);
        }
    }
}