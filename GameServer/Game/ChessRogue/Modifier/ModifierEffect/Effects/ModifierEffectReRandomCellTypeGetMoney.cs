using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum.ReRandomCellTypeGetMoney)]
public class ModifierEffectReRandomCellTypeGetMoney : ModifierEffectHandler
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
        var reRandomType = targetCell.BlockType;
        var refreshCell = chessRogueInstance.RogueCells.Values.Where(x =>
            x.BlockType == reRandomType && !x.IsCollapsed()).ToList();
        var types = modifierInstance.EffectConfig.Params.GetValueOrDefault("TargetType", "3").Split(";");
        var targetType = types.Select(x => (RogueDLCBlockTypeEnum)int.Parse(x)).ToList();

        foreach (var cell in refreshCell) cell.BlockType = targetType.RandomElement();

        // get money
        var money = int.Parse(modifierInstance.EffectConfig.Params.GetValueOrDefault("Count", "0"));
        await chessRogueInstance.GainMoney(money);

        await chessRogueInstance.Player.SendPacket(new PacketChessRogueCellUpdateNotify(refreshCell,
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