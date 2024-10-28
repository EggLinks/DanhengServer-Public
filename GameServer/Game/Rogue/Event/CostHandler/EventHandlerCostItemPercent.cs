using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.CostHandler;

[RogueEvent(costType: DialogueEventCostTypeEnum.CostItemPercent)]
public class EventHandlerCostItemPercent : RogueEventCostHandler
{
    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList)
    {
        await rogue.CostMoney((int)(rogue.CurMoney * (paramList[1] / 100f)));
    }
}