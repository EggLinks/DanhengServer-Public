using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier.ModifierEffect.Effects;

[ModifierEffect(ModifierEffectTypeEnum.AddMazeBuff)]
public class ModifierEffectAddMazeBuff : ModifierEffectHandler
{
    public override async ValueTask OnConfirmed(ChessRogueDiceModifierInstance modifierInstance,
        ChessRogueInstance chessRogueInstance)
    {
        modifierInstance.IsConfirmed = true;
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
        await ValueTask.CompletedTask;
    }

    public override void BeforeBattle(ChessRogueDiceModifierInstance modifierInstance, BattleInstance battle,
        ChessRogueInstance instance)
    {
        modifierInstance.EffectConfig.Params.TryGetValue("BuffId", out var buffId);
        if (buffId == null) return;

        var buff = new MazeBuff(int.Parse(buffId), 1, -1)
        {
            WaveFlag = -1
        };

        GameData.MazeBuffData.TryGetValue(int.Parse(buffId) * 10 + 1, out var buffExcel);
        if (buffExcel != null)
        {
            var modifier = GameData.AdventureModifierData.GetValueOrDefault(buffExcel.ModifierName);
            if (modifier != null)
                // handle modifier
                instance.HandleMazeBuffModifier(modifier, buff);
        }

        battle.Buffs.Add(buff);
    }

    public override async ValueTask AfterBattle(ChessRogueDiceModifierInstance modifierInstance, BattleInstance battle)
    {
        await ValueTask.CompletedTask;
    }
}