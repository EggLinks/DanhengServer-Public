using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect;

[AttributeUsage(AttributeTargets.Class)]
public class ModifierEffectAttribute(ModifierEffectTypeEnum effectType) : Attribute
{
    public ModifierEffectTypeEnum EffectType { get; } = effectType;
}