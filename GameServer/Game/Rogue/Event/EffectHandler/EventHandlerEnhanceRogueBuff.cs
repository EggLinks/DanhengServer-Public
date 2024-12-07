using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.EnhanceRogueBuff)]
public class EventHandlerEnhanceRogueBuff : RogueEventEffectHandler
{
    public override void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option)
    {
    }

    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList, RogueEventParam? option)
    {
        var group = paramList[0];
        GameData.RogueBuffGroupData.TryGetValue(group, out var buffGroup);
        if (buffGroup == null) return;
        var count = paramList[1];

        var buffs = rogue.GetRogueBuffInGroup(buffGroup.GetId());
        if (buffs.Count == 0) return;

        for (var i = 0; i < count;)
        {
            if (buffs.Count == 0) break; // No more buffs to enhance
            var buff = buffs.RandomElement();
            if (buff.BuffLevel == 2)
            {
                // Buff is already at max level
                buffs.Remove(buff);
                continue;
            }

            await rogue.EnhanceBuff(buff.BuffId);
            i++;
        }
    }
}