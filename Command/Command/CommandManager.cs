using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.GameServer.Command;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Util;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command
{
    public class CommandManager
    {
        public static CommandManager? Instance { get; private set; }
        public Dictionary<string, ICommand> Commands { get; } = [];
        public Dictionary<string, CommandInfo> CommandInfo { get; } = [];
        public Logger Logger { get; } = new Logger("CommandManager");
        public Connection? Target { get; set; } = null;

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
            Logger.Info($"Register {Commands.Count} commands.");
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    string? input = AnsiConsole.Ask<string>("> ");
                    if (string.IsNullOrEmpty(input))
                    {
                        continue;
                    }
                    HandleCommand(input, new ConsoleCommandSender(Logger));
                }
                catch
                {
                    Logger.Error(I18nManager.Translate("Game.Command.Notice.InternalError"));
                }
            }
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
                        var con = Listener.Connections.Values.ToList().Find(item => item.Player?.Uid.ToString() == target);
                        if (con != null)
                        {
                            Target = con;
                            sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetFound", target, con.Player!.Data.Name!));
                        }
                        else
                        {
                            // offline or not exist
                            sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetNotFound", target));
                        }
                        return;
                    }
                } else
                {
                    // player
                    tempTarget = Listener.GetActiveConnection(sender.GetSender());
                    if (tempTarget == null)
                    {
                        sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetNotFound", sender.GetSender().ToString()));
                        return;
                    }
                }

                if (tempTarget != null && !tempTarget.IsOnline)
                {
                    sender.SendMsg(I18nManager.Translate("Game.Command.Notice.TargetOffline", tempTarget.Player!.Uid.ToString(), tempTarget.Player!.Data.Name!));
                    tempTarget = null;
                }

                if (Commands.TryGetValue(cmd, out var command))
                {
                    var split = input.Split(' ').ToList();
                    split.RemoveAt(0);

                    var arg = new CommandArg(split.JoinFormat(" ", ""), sender, tempTarget);
                    // find the proper method with attribute CommandMethod
                    var isFound = false;
                    CommandInfo info = CommandInfo[cmd];

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
                            {
                                sender.SendMsg(I18nManager.Translate("Game.Command.Help.CommandUsage") + I18nManager.Translate(info.Usage));
                            }
                            else
                            {
                                sender.SendMsg(I18nManager.Translate("Game.Command.Notice.CommandNotFound"));
                            }
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
}
