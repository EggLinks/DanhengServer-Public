using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event;

[AttributeUsage(AttributeTargets.Class)]
public class RogueEventAttribute(
    DialogueEventTypeEnum effectType = DialogueEventTypeEnum.None,
    DialogueEventCostTypeEnum costType = DialogueEventCostTypeEnum.None) : Attribute
{
    public DialogueEventTypeEnum EffectType { get; } = effectType;
    public DialogueEventCostTypeEnum CostType { get; } = costType;
}