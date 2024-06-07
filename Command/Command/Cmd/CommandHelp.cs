using EggLink.DanhengServer.Internationalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("help", "Game.Command.Help.Desc", "Game.Command.Help.Usage")]
    public class CommandHelp : ICommand
    {
        [CommandDefault]
        public void Help(CommandArg arg)
        {
            var commands = CommandManager.Instance?.CommandInfo.Values;
            arg.SendMsg(I18nManager.Translate("Game.Command.Help.Commands"));
            if (commands == null)
            {
                return;
            }
            foreach (var command in commands)
            {
                arg.SendMsg($"/{command.Name} - {I18nManager.Translate(command.Description)}\n{I18nManager.Translate("Game.Command.Help.CommandUsage")} {I18nManager.Translate(command.Usage)}");
            }
        }
    }
}
