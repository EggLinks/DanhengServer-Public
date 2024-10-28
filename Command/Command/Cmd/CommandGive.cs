using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("give", "Game.Command.Give.Desc", "Game.Command.Give.Usage", ["g"])]
public class CommandGive : ICommand
{
    [CommandDefault]
    public async ValueTask GiveItem(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        if (arg.BasicArgs.Count == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        GameData.ItemConfigData.TryGetValue(arg.GetInt(0), out var itemData);
        if (itemData == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Give.ItemNotFound"));
            return;
        }

        arg.CharacterArgs.TryGetValue("x", out var str);
        arg.CharacterArgs.TryGetValue("l", out var levelStr);
        arg.CharacterArgs.TryGetValue("r", out var rankStr);
        str ??= "1";
        levelStr ??= "1";
        rankStr ??= "1";
        if (!int.TryParse(str, out var amount) || !int.TryParse(levelStr, out var level) ||
            !int.TryParse(rankStr, out var rank))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        await player.InventoryManager!.AddItem(arg.GetInt(0), amount, rank: Math.Min(rank, 5),
            level: Math.Max(Math.Min(level, 80), 1));

        await arg.SendMsg(I18NManager.Translate("Game.Command.Give.GiveItem", player.Uid.ToString(), amount.ToString(),
            itemData.Name ?? itemData.ID.ToString()));
    }
}