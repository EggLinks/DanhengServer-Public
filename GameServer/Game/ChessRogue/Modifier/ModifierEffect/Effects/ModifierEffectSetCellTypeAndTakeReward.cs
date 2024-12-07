using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum.SetCellTypeAndTakeReward)]
public class ModifierEffectSetCellTypeAndTakeReward : ModifierEffectHandler
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

        var cell = chessRogueInstance.RogueCells[selectCellId];
        var types = modifierInstance.EffectConfig.Params.GetValueOrDefault("TargetType", "3").Split(";");
        var targetType = types.Select(x => (RogueDLCBlockTypeEnum)int.Parse(x)).ToList().RandomElement();
        var rewards = modifierInstance.EffectConfig.Params.GetValueOrDefault("Reward", "").Split(";");
        foreach (var reward in rewards)
        {
            var param = reward.Split(",");
            if (param.Length != 2) continue;

            switch (param[0])
            {
                case "Area":
                    if (cell.BlockType == RogueDLCBlockTypeEnum.MonsterElite)
                    {
                        await chessRogueInstance.RollBuff(1, 100003);
                        await chessRogueInstance.GainMoney(Random.Shared.Next(70, 150), 2);
                    }
                    else
                    {
                        await chessRogueInstance.RollBuff(1, 100004);
                        await chessRogueInstance.GainMoney(Random.Shared.Next(40, 60), 2);
                    }

                    break;
                case "Event":
                    var eventCount = cell.MarkType switch
                    {
                        RogueCellMarkTypeEnum.Choice => 3,
                        RogueCellMarkTypeEnum.Double => 2,
                        _ => 1
                    };

                    switch (param[1])
                    {
                        case "Buff":
                            await chessRogueInstance.RollBuff(eventCount, 100004);
                            break;
                        case "Money":
                            await chessRogueInstance.GainMoney(
                                int.Parse(modifierInstance.EffectConfig.Params.GetValueOrDefault("Count", "10")), 2);
                            break;
                    }

                    break;
            }
        }

        cell.BlockType = targetType;

        await chessRogueInstance.Player.SendPacket(new PacketChessRogueCellUpdateNotify(cell,
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