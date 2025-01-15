using System.Reflection;
using EggLink.DanhengServer.GameServer.Server;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Util;
using Spectre.Console;
using static EggLink.DanhengServer.GameServer.Plugin.Event.PluginEvent;

namespace EggLink.DanhengServer.Command.Command;

public class CommandManager
{
    private const int MaxCommandHistory = 100;

    private readonly List<string> _commandHistory = [];
    private int _historyIndex = -1;
    public static CommandManager? Instance { get; private set; }
    public Dictionary<string, ICommand> Commands { get; } = [];
    public Dictionary<string, CommandInfoAttribute> CommandInfo { get; } = [];
    public Dictionary<string, string> CommandAlias { get; } = []; // alias -> command
    public Logger Logger { get; } = new("CommandManager");
    public Connection? Target { get; set; }

    public void RegisterCommands()
    {
        Instance = this;
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            if (typeof(ICommand).IsAssignableFrom(type) && !type.IsAbstract)
                RegisterCommand(type);

        Logger.Info(I18NManager.Translate("Server.ServerInfo.RegisterItem", Commands.Count.ToString(),
            I18NManager.Translate("Word.Command")));
    }

    public void RegisterCommand(Type type)
    {
        var attr = type.GetCustomAttribute<CommandInfoAttribute>();
        if (attr == null) return;
        var instance = Activator.CreateInstance(type);
        if (instance is not ICommand command) return;
        Commands.Add(attr.Name, command);
        CommandInfo.Add(attr.Name, attr);

        // register alias
        foreach (var alias in attr.Alias) // add alias
            CommandAlias.Add(alias, attr.Name);
    }

    public void Start()
    {
        while (true)
            try
            {
                var input = ReadCommand();

                if (string.IsNullOrEmpty(input)) continue;

                if (input.StartsWith("/")) input = input[1..];

                if (_commandHistory.Count >= MaxCommandHistory) _commandHistory.RemoveAt(0);

                if (_commandHistory.Count == 0 || _commandHistory.Last() != input) _commandHistory.Add(input);
                _historyIndex = _commandHistory.Count;
                HandleCommand(input, new ConsoleCommandSender(Logger));
            }
            catch
            {
                Logger.Error(I18NManager.Translate("Game.Command.Notice.InternalError"));
            }
        // ReSharper disable once FunctionNeverReturns
    }

    private string ReadCommand()
    {
        var input = new List<char>();

        AnsiConsole.Markup("[yellow]> [/]");
        while (true)
        {
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            switch (keyInfo.Key)
            {
                case ConsoleKey.Backspace:
                {
                    if (input.Count > 0)
                    {
                        input.RemoveAt(input.Count - 1);
                        Console.Write("\b \b");
                    }

                    break;
                }
                case ConsoleKey.UpArrow:
                {
                    if (_historyIndex > 0)
                    {
                        _historyIndex--;
                        ReplaceInput(input, _commandHistory[_historyIndex]);
                    }

                    break;
                }
                case ConsoleKey.DownArrow when _historyIndex < _commandHistory.Count - 1:
                    _historyIndex++;
                    ReplaceInput(input, _commandHistory[_historyIndex]);
                    break;
                case ConsoleKey.DownArrow:
                {
                    if (_historyIndex == _commandHistory.Count - 1)
                    {
                        _historyIndex++;
                        ReplaceInput(input, string.Empty);
                    }

                    break;
                }
                // known issue: Ctrl + (Any Key but C) or other control key will cause display error
                default:
                    input.Add(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                    break;
            }
        }

        return new string(input.ToArray());
    }

    private static void ReplaceInput(List<char> input, string newText)
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
                        sender.SendMsg(I18NManager.Translate("Game.Command.Notice.TargetFound", target,
                            con.Player!.Data.Name!));
                    }
                    else
                    {
                        // offline or not exist
                        sender.SendMsg(I18NManager.Translate("Game.Command.Notice.TargetNotFound", target));
                    }

                    return;
                }
            }
            else
            {
                InvokeOnPlayerUseCommand(sender, input);
                // player 
                tempTarget = Listener.GetActiveConnection(sender.GetSender());
                if (tempTarget == null)
                {
                    sender.SendMsg(I18NManager.Translate("Game.Command.Notice.TargetNotFound",
                        sender.GetSender().ToString()));
                    return;
                }
            }

            if (tempTarget is { IsOnline: false })
            {
                sender.SendMsg(I18NManager.Translate("Game.Command.Notice.TargetOffline",
                    tempTarget.Player!.Uid.ToString(), tempTarget.Player!.Data.Name!));
                tempTarget = null;
            }

            // find the command
            if (CommandAlias.TryGetValue(cmd, out var realCmd)) cmd = realCmd;

            if (Commands.TryGetValue(cmd, out var command))
            {
                var split = input.Split(' ').ToList();
                split.RemoveAt(0);

                var arg = new CommandArg(split.JoinFormat(" ", ""), sender, tempTarget);

                // judge permission
                if (arg.Target?.Player?.Uid != sender.GetSender() && !sender.HasPermission("command.others"))
                {
                    sender.SendMsg(I18NManager.Translate("Game.Command.Notice.NoPermission"));
                    return;
                }

                // find the proper method with attribute CommandMethodAttribute
                var isFound = false;
                var info = CommandInfo[cmd];

                if (!sender.HasPermission(info.Permission))
                {
                    sender.SendMsg(I18NManager.Translate("Game.Command.Notice.NoPermission"));
                    return;
                }

                foreach (var method in command.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<CommandMethodAttribute>();
                    if (attr == null) continue;
                    var canRun = true;
                    foreach (var condition in attr.Conditions)
                    {
                        if (split.Count <= condition.Index)
                        {
                            canRun = false;
                            break;
                        }

                        if (split[condition.Index].Equals(condition.ShouldBe)) continue;
                        canRun = false;
                        break;
                    }

                    if (!canRun) continue;
                    isFound = true;
                    method.Invoke(command, [arg]);
                    break;
                }

                if (isFound) return;
                // find the default method with attribute CommandDefaultAttribute
                foreach (var method in command.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<CommandDefaultAttribute>();
                    if (attr == null) continue;
                    isFound = true;
                    method.Invoke(command, [arg]);
                    break;
                }

                if (isFound) return;
                sender.SendMsg(I18NManager.Translate(info.Usage));
            }
            else
            {
                sender.SendMsg(I18NManager.Translate("Game.Command.Notice.CommandNotFound"));
            }
        }
        catch
        {
            sender.SendMsg(I18NManager.Translate("Game.Command.Notice.InternalError"));
        }
    }
}