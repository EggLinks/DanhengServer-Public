using EggLink.DanhengServer.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Command
{
    public static class CommandExecutor
    {
        public delegate void RunCommand(ICommandSender sender, string cmd);
        public static event RunCommand? OnRunCommand;

        public static void ExecuteCommand(ICommandSender sender, string cmd)
        {
            OnRunCommand?.Invoke(sender, cmd);
        }
    }
}
