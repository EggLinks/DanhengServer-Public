using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum.SwapCellToCurAround)]
public class ModifierEffectSwapCellToCurAround : ModifierEffectHandler
{
    public override async ValueTask OnConfirmed(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance)
    {
        var types = modifierInstance.EffectConfig.Params.GetValueOrDefault("SourceType", "3").Split(";");

        foreach (var type in types)
        {
            var cells = chessRogueInstance.RogueCells.Where(x =>
                x.Value.BlockType == (RogueDLCBlockTypeEnum)int.Parse(type) && !x.Value.IsCollapsed());
            modifierInstance.SelectableCells.AddRange(cells.Select(x => x.Key));
        }

        await ValueTask.CompletedTask;
    }

    public override async ValueTask SelectModifierCell(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance,
        int selectCellId)
    {
        await chessRogueInstance.Player.SendPacket(
            new PacketRogueModifierStageStartNotify(modifierInstance.SourceType));
        modifierInstance.SelectedCell = selectCellId;
        modifierInstance.IsConfirmed = true;

        var targetCell = chessRogueInstance.RogueCells[selectCellId];
        // get the cells around the current cell
        var targetCells = chessRogueInstance.RogueCells.Values.Where(x =>
            Math.Abs(x.PosX - targetCell.PosX) <= 1 && Math.Abs(x.PosY - targetCell.PosY) <= 1 &&
            x.CellAdvanceInfo.Count == 0 && !x.IsCollapsed()).ToList(); // avoid to get boss cell
        targetCells.Remove(targetCell);

        List<ChessRogueCellInstance> updated = [];

        // swap the cell type
        if (targetCells.Count == 0) return; // no cell to swap
        var cell = targetCells.RandomElement();
        (cell.BlockType, targetCell.BlockType) = (targetCell.BlockType, cell.BlockType);
        updated.Add(cell);
        updated.Add(targetCell);

        // Send packet to update the cell
        await chessRogueInstance.Player.SendPacket(new PacketChessRogueCellUpdateNotify(updated,
            chessRogueInstance.CurBoardExcel?.ChessBoardID ?? 0, modifierInstance.SourceType,
            ChessRogueCellUpdateReason.Modifier));
    }

    public override async ValueTask SelectCell(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance,
        int selectCellId)
    {
        await ValueTask.CompletedTask;
    }

    public override void BeforeBattle(ChessRogueDiceModifierInstance modifierInstance, BattleInstance battle,
        ChessRogueInstance instance)
    {
    }

    public override async ValueTask AfterBattle(ChessRogueDiceModifierInstance modifierInstance, BattleInstance battle)
    {
        await ValueTask.CompletedTask;
    }
}