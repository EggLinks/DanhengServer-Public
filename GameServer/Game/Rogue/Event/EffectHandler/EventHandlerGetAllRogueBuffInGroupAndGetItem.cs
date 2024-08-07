using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.GetAllRogueBuffInGroupAndGetItem)]
public class EventHandlerGetAllRogueBuffInGroupAndGetItem : RogueEventEffectHandler
{
    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList)
    {
        var group = paramList[0];
        GameData.RogueBuffGroupData.TryGetValue(group, out var buffGroup);
        if (buffGroup == null) return;
        await rogue.AddBuffList(buffGroup.BuffList);
        await rogue.GainMoney(paramList[2], paramList[3],
            RogueCommonActionResultDisplayType.Single);
    }
}