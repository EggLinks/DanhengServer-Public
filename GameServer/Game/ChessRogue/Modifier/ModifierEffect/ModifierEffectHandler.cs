using EggLink.DanhengServer.GameServer.Game.Battle;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect;

public abstract class ModifierEffectHandler
{
    public abstract ValueTask OnConfirmed(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance);

    public abstract ValueTask SelectModifierCell(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance, int selectCellId);

    public abstract ValueTask SelectCell(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance, int selectCellId);

    public abstract void BeforeBattle(ChessRogueDiceModifierInstance modifierInstance, BattleInstance battle,
        ChessRogueInstance instance);

    public abstract ValueTask AfterBattle(ChessRogueDiceModifierInstance modifierInstance, BattleInstance battle);
}