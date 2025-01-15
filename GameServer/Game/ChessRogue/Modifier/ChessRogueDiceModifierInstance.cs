using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier;

public class ChessRogueDiceModifierInstance(int modifierId, ChessRogueDiceSurfaceContentEffect effect)
{
    public int ModifierId { get; set; } = modifierId;
    public ChessRogueDiceSurfaceContentEffect EffectConfig { get; set; } = effect;
    public RogueModifierSourceType SourceType { get; set; } = RogueModifierSourceType.RogueModifierSourceDiceRoll;
    public List<int> SelectableCells { get; set; } = [];
    public int SelectedCell { get; set; }
    public bool IsConfirmed { get; set; }

    public RogueModifier ToProto()
    {
        return new RogueModifier
        {
            ModifierId = (ulong)ModifierId,
            ModifierSourceType = SourceType,
            ModifierContent = new RogueModifierContent
            {
                ContentModifierEffectId = (uint)EffectConfig.EffectType,
                ModifierContentType = RogueModifierContentType.RogueModifierContentDefinite
            },
            ModifierInfo = new ChessRogueModifierInfo
            {
                ModifierEffectCellIdList = { SelectableCells.Select(x => (uint)x) },
                SelectCellId = (uint)SelectedCell,
                Confirm = IsConfirmed
            }
        };
    }

    #region Effect

    public async ValueTask SelectCell(ChessRogueInstance instance, int selectCellId)
    {
        var effect = EffectConfig.EffectType;

        instance.ModifierEffectHandlers.TryGetValue(effect, out var handler);

        if (handler != null)
            await handler.SelectCell(this, instance, selectCellId);
        else
            IsConfirmed = true;
    }

    public async ValueTask SelectModifierCell(ChessRogueInstance instance, int selectCellId)
    {
        var effect = EffectConfig.EffectType;

        instance.ModifierEffectHandlers.TryGetValue(effect, out var handler);

        if (handler != null)
            await handler.SelectModifierCell(this, instance, selectCellId);
        else
            IsConfirmed = true;
    }

    public async ValueTask OnConfirmed(ChessRogueInstance instance)
    {
        var effect = EffectConfig.EffectType;

        instance.ModifierEffectHandlers.TryGetValue(effect, out var handler);

        if (handler != null)
            await handler.OnConfirmed(this, instance);
        else
            IsConfirmed = true;
    }

    public void BeforeBattle(ChessRogueInstance instance, BattleInstance battle)
    {
        var effect = EffectConfig.EffectType;

        instance.ModifierEffectHandlers.TryGetValue(effect, out var handler);

        if (handler != null)
            handler.BeforeBattle(this, battle, instance);
        else
            IsConfirmed = true;
    }

    public async ValueTask AfterBattle(ChessRogueInstance instance, BattleInstance battle)
    {
        var effect = EffectConfig.EffectType;

        instance.ModifierEffectHandlers.TryGetValue(effect, out var handler);

        if (handler != null)
            await handler.AfterBattle(this, battle);
        else
            IsConfirmed = true;
    }

    #endregion
}