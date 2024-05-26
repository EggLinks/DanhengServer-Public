using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.EnhanceRogueBuff)]
    public class EventHandlerEnhanceRogueBuff : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            var group = ParamList[0];
            GameData.RogueBuffGroupData.TryGetValue(group, out var buffGroup);
            if (buffGroup == null) return;
            var count = ParamList[1];

            var buffs = rogue.GetRogueBuffInGroup(buffGroup.GroupID);
            if (buffs == null) return;

            for (int i = 0; i < count;)
            {
                if (buffs.Count == 0) break;  // No more buffs to enhance
                var buff = buffs.RandomElement();
                if (buff == null) break;
                if (buff.BuffLevel == 2) 
                {
                    // Buff is already at max level
                    buffs.Remove(buff);
                    continue;
                }
                rogue.EnhanceBuff(buff.BuffId);
                i++;
            }
        }
    }
}
