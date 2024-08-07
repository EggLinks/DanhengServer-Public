using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.CostHandler;

[RogueEvent(costType: DialogueEventCostTypeEnum.CostItemValue)]
public class EventHandlerCostItemValue : RogueEventCostHandler
{
    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList)
    {
        var decreaseMoney = paramList[1];
        await rogue.CostMoney(decreaseMoney, RogueCommonActionResultDisplayType.Single);
    }
}