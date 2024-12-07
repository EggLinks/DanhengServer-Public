using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum.SetBlockTypeToAround)]
public class ModifierEffectSetBlockTypeToAround : ModifierEffectHandler
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
        var count = int.Parse(modifierInstance.EffectConfig.Params.GetValueOrDefault("Count", "1"));
        List<ChessRogueCellInstance> targetCells = [];
        var types = modifierInstance.EffectConfig.Params.GetValueOrDefault("SourceType", "3").Split(";");
        foreach (var type in types)
        {
            var cells = chessRogueInstance.RogueCells.Where(x =>
                x.Value.BlockType == (RogueDLCBlockTypeEnum)int.Parse(type) && !x.Value.IsCollapsed() &&
                Math.Abs(x.Value.PosX - targetCell.PosX) <= 1 && Math.Abs(x.Value.PosY - targetCell.PosY) <= 1);

            targetCells.AddRange(cells.Select(x => x.Value));
        }

        targetCells.Remove(targetCell);

        List<ChessRogueCellInstance> updated = [];
        for (var i = 0; i < count; i++)
        {
            if (targetCells.Count == 0)
                break;
            var cell = targetCells.RandomElement();
            cell.BlockType = targetCell.BlockType;
            targetCells.Remove(cell);
            updated.Add(cell);
        }

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