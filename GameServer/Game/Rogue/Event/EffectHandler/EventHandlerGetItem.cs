using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.GetItem)]
public class EventHandlerGetItem : RogueEventEffectHandler
{
    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList)
    {
        await rogue.GainMoney(paramList[1], paramList[2],
            RogueCommonActionResultDisplayType.Single);
    }
}