using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum
    .TrunAroundToEmptyGetBuff)] // I don't know why it's called TrunAround instead of TurnAround, maybe miHoYo r sleeping lol
public class ModifierEffectTrunAroundToEmptyGetBuff : ModifierEffectHandler
{
    public override async ValueTask OnConfirmed(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance)
    {
        await ValueTask.CompletedTask;
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
        await chessRogueInstance.Player.SendPacket(
            new PacketRogueModifierStageStartNotify(modifierInstance.SourceType));
        var targetCell = chessRogueInstance.RogueCells[selectCellId];
        modifierInstance.IsConfirmed = true;

        var types = modifierInstance.EffectConfig.Params.GetValueOrDefault("SourceType", "3").Split(";");

        List<ChessRogueCellInstance> targetCells = [];
        var count = 0;
        foreach (var type in types)
        {
            var cells = chessRogueInstance.RogueCells.Where(x =>
                x.Value.BlockType == (RogueDLCBlockTypeEnum)int.Parse(type) && !x.Value.IsCollapsed() &&
                Math.Abs(x.Value.PosX - targetCell.PosX) <= 1 && Math.Abs(x.Value.PosY - targetCell.PosY) <= 1);

            targetCells.AddRange(cells.Select(x => x.Value));
        }

        foreach (var cell in targetCells)
        {
            if (cell.BlockType != RogueDLCBlockTypeEnum.Empty)
                count++;
            cell.BlockType = RogueDLCBlockTypeEnum.Empty;
        }

        await chessRogueInstance.Player.SendPacket(new PacketChessRogueCellUpdateNotify(targetCells,
            chessRogueInstance.CurBoardExcel?.ChessBoardID ?? 0, modifierInstance.SourceType,
            ChessRogueCellUpdateReason.Modifier));

        if (count > 0) await chessRogueInstance.RollBuff(count, 100004);
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