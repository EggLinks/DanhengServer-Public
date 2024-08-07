using System.Reflection;
using EggLink.DanhengServer.GameServer.Server;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Util;
using Spectre.Console;

namespace EggLink.DanhengServer.Command.Command;

public class CommandManager
{
    private const int MaxCommandHistory = 100;

    private readonly List<string> _commandHistory = [];
    private int _historyIndex = -1;
    public static CommandManager? Instance { get; private set; }
    public Dictionary<string, ICommand> Commands { get; } = [];
    public Dictionary<string, CommandInfo> CommandInfo { get; } = [];
    public Logger Logger { get; } = new("CommandManager");
    public Connection? Target { get; set; }

    public void RegisterCommand()
    {
        Instance = this;
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attr = type.GetCustomAttribute<CommandInfo>();
            if (attr != null)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is ICommand command)
                {
                    Commands.Add(attr.Name, command);
                    CommandInfo.Add(attr.Name, attr);
                }
            }
        }

        Logger.Info(I18nManager.Translate("Server.ServerInfo.RegisterItem", Commands.Count.ToString(),
            I18nManager.Translate("Word.Command")));
    }

    public void Start()
    {
        while (true)
            try
            {
                var input = ReadCommand();

                if (string.IsNullOrEmpty(input)) continue;

                if (input.StartsWith("/")) input = input.Substring(1);

                if (_commandHistory.Count >= MaxCommandHistory) _commandHistory.RemoveAt(0);

                if (_commandHistory.Count == 0 || _commandHistory.Last() != input) _commandHistory.Add(input);
                _historyIndex = _commandHistory.Count;
                HandleCommand(input, new ConsoleCommandSender(Logger));
            }
            catch
            {
                Logger.Error(I18nManager.Translate("Game.Command.Notice.InternalError"));
            }
    }

    private string ReadCommand()
    {
        var input = new List<char>();
        ConsoleKeyInfo keyInfo;

        AnsiConsole.Markup("[yellow]> [/]");
        while (true)
        {
            keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (input.Count > 0)
                {
                    input.RemoveAt(input.Count - 1);
                    Console.Write("\b \b");
                }
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (_historyIndex > 0)
                {
                    _historyIndex--;
                    ReplaceInput(input, _commandHistory[_historyIndex]);
                }
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                if (_historyIndex < _commandHistory.Count - 1)
                {
                    _historyIndex++;
                    ReplaceInput(input, _commandHistory[_historyIndex]);
                }
                else if (_historyIndex == _commandHistory.Count - 1)
                {
                    _historyIndex++;
                    ReplaceInput(input, string.Empty);
                }
            }
            else // known issue: Ctrl + (Any Key but C) or other control key will cause display error
            {
                input.Add(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
        }

        return new string(input.ToArray());
    }

    private void ReplaceInput(List<char> input, string newText)
    {
        while (input.Count > 0)
        {
            input.RemoveAt(input.Count - 1);
            Console.Write("\b \b");
        }

        input.AddRange(newText.ToCharArray());
        Console.Write(newText);
    }

    public void HandleCommand(string input, ICommandSender sender)
    {
        try
        {
            var cmd = input.Split(' ')[0];

            var tempTarget = Target;
            if (sender is ConsoleCommandSender)
            {
                if (cmd.StartsWith('@'))
                {
                    var target = cmd[1..];
                    if (DanhengListener.Connections.Values.ToList()
                            .Find(item => (item as Connection)?.Player?.Uid.ToString() == target) is Connection con)
                    {
                        Target = con;
                        sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetFound", target,
                            con.Player!.Data.Name!));
                    }
                    else
                    {
                        // offline or not exist
                        sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetNotFound", target));
                    }

                    return;
                }
            }
            else
            {
                // player 
                tempTarget = Listener.GetActiveConnection(sender.GetSender());
                if (tempTarget == null)
                {
                    sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetNotFound",
                        sender.GetSender().ToString()));
                    return;
                }
            }

            if (tempTarget != null && !tempTarget.IsOnline)
            {
                sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetOffline",
                    tempTarget.Player!.Uid.ToString(), tempTarget.Player!.Data.Name!));
                tempTarget = null;
            }

            if (Commands.TryGetValue(cmd, out var command))
            {
                var split = input.Split(' ').ToList();
                split.RemoveAt(0);

                var arg = new CommandArg(split.JoinFormat(" ", ""), sender, tempTarget);

                // judge permission
                if (arg.Target?.Player?.Uid != sender.GetSender() && !sender.HasPermission("command.others"))
                {
                    sender.SendMsg(I18nManager.Translate("Game.Command.Notice.NoPermission"));
                    return;
                }

                // find the proper method with attribute CommandMethod
                var isFound = false;
                var info = CommandInfo[cmd];

                if (!sender.HasPermission(info.Permission))
                {
                    sender.SendMsg(I18nManager.Translate("Game.Command.Notice.NoPermission"));
                    return;
                }

                foreach (var method in command.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<CommandMethod>();
                    if (attr != null)
                    {
                        var canRun = true;
                        foreach (var condition in attr.Conditions)
                        {
                            if (split.Count <= condition.Index)
                            {
                                canRun = false;
                                break;
                            }

                            if (!split[condition.Index].Equals(condition.ShouldBe))
                            {
                                canRun = false;
                                break;
                            }
                        }

                        if (canRun)
                        {
                            isFound = true;
                            method.Invoke(command, [arg]);
                            break;
                        }
                    }
                }

                if (!isFound)
                {
                    // find the default method with attribute CommandDefault
                    foreach (var method in command.GetType().GetMethods())
                    {
                        var attr = method.GetCustomAttribute<CommandDefault>();
                        if (attr != null)
                        {
                            isFound = true;
                            method.Invoke(command, [arg]);
                            break;
                        }
                    }

                    if (!isFound)
                    {
                        if (info != null)
                            sender.SendMsg(I18nManager.Translate(info.Usage));
                        else
                            sender.SendMsg(I18nManager.Translate("Game.Command.Notice.CommandNotFound"));
                    }
                }
            }
            else
            {
                sender.SendMsg(I18nManager.Translate("Game.Command.Notice.CommandNotFound"));
            }
        }
        catch
        {
            sender.SendMsg(I18nManager.Translate("Game.Command.Notice.InternalError"));
        }
    }
}