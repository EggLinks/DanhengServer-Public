using EggLink.DanhengServer.Enums.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RogueEventAttribute(DialogueEventTypeEnum effectType = DialogueEventTypeEnum.None, DialogueEventCostTypeEnum costType = DialogueEventCostTypeEnum.None) : Attribute
    {
        public DialogueEventTypeEnum EffectType { get; } = effectType;
        public DialogueEventCostTypeEnum CostType { get; } = costType;
    }
}
