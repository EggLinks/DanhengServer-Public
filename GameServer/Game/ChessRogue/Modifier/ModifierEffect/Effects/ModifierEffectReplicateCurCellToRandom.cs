using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum.ReplicateCurCellToRandom)]
public class ModifierEffectReplicateCurCellToRandom : ModifierEffectHandler
{
    public override async ValueTask OnConfirmed(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance)
    {
        await chessRogueInstance.Player.SendPacket(
            new PacketRogueModifierStageStartNotify(modifierInstance.SourceType));

        List<ChessRogueCellInstance> targetCells = []; // list of cells can be changed
        var types = modifierInstance.EffectConfig.Params.GetValueOrDefault("SourceType", "3")
            .Split(";"); // get the target types
        var count = int.Parse(
            modifierInstance.EffectConfig.Params.GetValueOrDefault("Count", "1")); // get the count of cells to change
        var curCell = chessRogueInstance.CurCell;
        foreach (var type in types)
        {
            var cells = chessRogueInstance.RogueCells.Where(x => // get all cells with the target type
                x.Value.BlockType == (RogueDLCBlockTypeEnum)int.Parse(type) && !x.Value.IsCollapsed());

            targetCells.AddRange(cells.Select(x => x.Value)); // add the cells to the list
        }

        List<ChessRogueCellInstance> updated = [];
        for (var i = 0; i < count; i++)
        {
            if (targetCells.Count == 0) // if there are no more cells to change, quit the loop
                break;
            var targetCell = targetCells.RandomElement(); // get a random cell from the list
            targetCell.BlockType =
                curCell?.BlockType ?? RogueDLCBlockTypeEnum.Empty; // set the cell type to the current cell type
            targetCells.Remove(targetCell);
            updated.Add(targetCell);
        }

        await chessRogueInstance.Player.SendPacket(new PacketChessRogueCellUpdateNotify(updated,
            chessRogueInstance.CurBoardExcel?.ChessBoardID ?? 0, modifierInstance.SourceType,
            ChessRogueCellUpdateReason.Modifier));

        modifierInstance.IsConfirmed = true;
    }

    public override async ValueTask SelectModifierCell(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance,
        int selectCellId)
    {
        await ValueTask.CompletedTask;
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