using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("rogue", "Game.Command.Rogue.Desc", "Game.Command.Rogue.Usage", ["r"])]
public class CommandRogue : ICommand
{
    [CommandMethod("0 money")]
    public async ValueTask GetMoney(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var count = arg.GetInt(0);
        var instance = arg.Target.Player!.RogueManager?.GetRogueInstance();
        if (instance == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerNotInRogue"));
            return;
        }

        await instance.GainMoney(count);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerGainedMoney", count.ToString()));
    }

    [CommandMethod("0 buff")]
    public async ValueTask GetBuff(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var instance = arg.Target.Player!.RogueManager?.GetRogueInstance();
        if (instance == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerNotInRogue"));
            return;
        }

        var id = arg.GetInt(0);

        if (id == -1)
        {
            var buffList = instance.RogueSubMode != RogueSubModeEnum.TournRogue
                ? GameData.RogueBuffData.Values.Where(buff =>
                        buff is RogueBuffExcel { IsAeonBuff: false } && buff.MazeBuffLevel != 2)
                    .ToList()
                : GameData.RogueBuffData.Values.Where(buff =>
                        buff is RogueTournBuffExcel && buff.MazeBuffLevel != 2)
                    .ToList();

            await instance.AddBuffList(buffList);

            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerGainedAllItems",
                I18NManager.Translate("Word.Buff")));
        }
        else
        {
            GameData.RogueMazeBuffData.TryGetValue(id, out var buff);
            if (buff == null)
            {
                await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.ItemNotFound",
                    I18NManager.Translate("Word.Buff")));
                return;
            }

            await instance.AddBuff(buff.ID, buff.Lv);

            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerGainedItem",
                I18NManager.Translate("Word.Buff"), buff.Name ?? id.ToString()));
        }
    }

    [CommandMethod("0 miracle")]
    public async ValueTask GetMiracle(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var instance = arg.Target.Player!.RogueManager?.GetRogueInstance();
        if (instance == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerNotInRogue"));
            return;
        }

        var id = arg.GetInt(0);

        GameData.RogueMiracleData.TryGetValue(id, out var miracle);
        if (miracle == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.ItemNotFound",
                I18NManager.Translate("Word.Miracle")));
            return;
        }

        await instance.AddMiracle(id);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerGainedItem",
            I18NManager.Translate("Word.Miracle"), miracle.Name ?? id.ToString()));
    }

    [CommandMethod("0 enhance")]
    public async ValueTask GetEnhance(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var instance = arg.Target.Player!.RogueManager?.GetRogueInstance();
        if (instance == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerNotInRogue"));
            return;
        }

        var id = arg.GetInt(0);
        if (id == -1)
        {
            foreach (var enhance in GameData.RogueBuffData.Values) await instance.EnhanceBuff(enhance.MazeBuffID);
            await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerEnhancedAllBuffs"));
        }
        else
        {
            GameData.RogueMazeBuffData.TryGetValue(id, out var buff);
            if (buff == null)
            {
                await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.ItemNotFound",
                    I18NManager.Translate("Word.Buff")));
                return;
            }

            await instance.EnhanceBuff(buff.ID);
            await arg.SendMsg(
                I18NManager.Translate("Game.Command.Rogue.PlayerEnhancedBuff", buff.Name ?? id.ToString()));
        }
    }

    [CommandMethod("0 unstuck")]
    public async ValueTask Unstuck(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var player = arg.Target.Player!;
        foreach (var npc in player.SceneInstance!.Entities.Values)
            if (npc is RogueNpc { RogueNpcId: > 0 } rNpc)
                await player.SceneInstance!.RemoveEntity(rNpc);

        await arg.SendMsg(I18NManager.Translate("Game.Command.Rogue.PlayerUnstuck"));
    }
}