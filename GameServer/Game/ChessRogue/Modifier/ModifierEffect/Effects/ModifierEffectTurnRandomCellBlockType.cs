using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum.TurnRandomCellBlockType)]
public class ModifierEffectTurnRandomCellBlockType : ModifierEffectHandler
{
    public override async ValueTask OnConfirmed(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance)
    {
        await chessRogueInstance.Player.SendPacket(
            new PacketRogueModifierStageStartNotify(modifierInstance.SourceType));

        List<ChessRogueCellInstance> targetCells = [];
        var types = modifierInstance.EffectConfig.Params.GetValueOrDefault("SourceType", "3").Split(";");
        var targetTypes = modifierInstance.EffectConfig.Params.GetValueOrDefault("TargetType", "3").Split(";")
            .Select(x => (RogueDLCBlockTypeEnum)int.Parse(x)).ToList();
        var count = int.Parse(modifierInstance.EffectConfig.Params.GetValueOrDefault("Count", "1"));
        foreach (var type in types)
        {
            var cells = chessRogueInstance.RogueCells.Where(x =>
                x.Value.BlockType == (RogueDLCBlockTypeEnum)int.Parse(type) && !x.Value.IsCollapsed());

            targetCells.AddRange(cells.Select(x => x.Value));
        }

        List<ChessRogueCellInstance> updated = [];
        for (var i = 0; i < count; i++)
        {
            if (targetCells.Count == 0)
                break;
            var targetCell = targetCells.RandomElement();
            targetCell.BlockType = targetTypes.RandomElement();
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