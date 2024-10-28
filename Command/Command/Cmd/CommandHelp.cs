using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("help", "Game.Command.Help.Desc", "Game.Command.Help.Usage", ["h"])]
public class CommandHelp : ICommand
{
    [CommandDefault]
    public async ValueTask Help(CommandArg arg)
    {
        var commands = CommandManager.Instance?.CommandInfo;
        if (arg.Args.Count == 1)
        {
            var cmd = arg.Args[0];
            if (commands == null || !commands.TryGetValue(cmd, out var command))
            {
                await arg.SendMsg(I18NManager.Translate("Game.Command.Help.CommandNotFound"));
                return;
            }

            var msg =
                $"/{command.Name} - {I18NManager.Translate(command.Description)}\n\n{I18NManager.Translate(command.Usage)}";
            if (command.Alias.Length > 0)
                msg +=
                    $"\n\n{I18NManager.Translate("Game.Command.Help.CommandAlias")} {command.Alias.ToList().ToArrayString()}";
            if (command.Permission != "")
                msg += $"\n\n{I18NManager.Translate("Game.Command.Help.CommandPermission")} {command.Permission}";

            await arg.SendMsg(msg);
            return;
        }

        await arg.SendMsg(I18NManager.Translate("Game.Command.Help.Commands"));
        if (commands == null) return;

        foreach (var command in commands.Values)
        {
            var msg =
                $"/{command.Name} - {I18NManager.Translate(command.Description)}\n\n{I18NManager.Translate(command.Usage)}";
            if (command.Alias.Length > 0)
                msg +=
                    $"\n\n{I18NManager.Translate("Game.Command.Help.CommandAlias")} {command.Alias.ToList().ToArrayString()}";

            if (command.Permission != "")
                msg += $"\n\n{I18NManager.Translate("Game.Command.Help.CommandPermission")} {command.Permission}";
            await arg.SendMsg(msg);
        }
    }
}