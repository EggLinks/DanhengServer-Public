using EggLink.DanhengServer.Command;

namespace EggLink.DanhengServer.GameServer.Command;

public static class CommandExecutor
{
    public delegate void RunCommand(ICommandSender sender, string cmd);

    public static event RunCommand? OnRunCommand;

    public static void ExecuteCommand(ICommandSender sender, string cmd)
    {
        OnRunCommand?.Invoke(sender, cmd);
    }
}