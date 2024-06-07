using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.GetAllRogueBuffInGroupAndGetItem)]
    public class EventHandlerGetAllRogueBuffInGroupAndGetItem : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            var group = ParamList[0];
            GameData.RogueBuffGroupData.TryGetValue(group, out var buffGroup);
            if (buffGroup == null) return;
            rogue.AddBuffList(buffGroup.BuffList);
            rogue.GainMoney(ParamList[2], ParamList[3], Proto.RogueActionDisplayType.RogueCommonActionResultDisplayTypeSingle);
        }
    }
}
