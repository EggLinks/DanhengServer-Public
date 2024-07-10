using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Configuration;
using EggLink.DanhengServer.WebServer;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Server;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EggLink.DanhengServer.Handbook;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Plugin;
using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.Server.Packet;
using EggLink.DanhengServer.GameServer.Command;
using EggLink.DanhengServer.WebServer.Server;
using EggLink.DanhengServer.Enums;

namespace EggLink.DanhengServer.Program
{
    public partial class EntryPoint
    {
        private readonly static Logger logger = new("Program");
        public readonly static DatabaseHelper DatabaseHelper = new();
        public readonly static Listener Listener = new();
        public readonly static CommandManager CommandManager = new();

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => {
                Console.WriteLine("Shutting down...");
                PerformCleanup();
            };
            Console.CancelKeyPress += (sender, eventArgs) => {
                Console.WriteLine("Cancel key pressed. Shutting down...");
                eventArgs.Cancel = true;
                Environment.Exit(0);
            };
            var time = DateTime.Now;
            // Initialize the logfile
            var counter = 0;
            FileInfo file;
            while (true)
            {
                file = new FileInfo(GetConfig().Path.LogPath + $"/{DateTime.Now:yyyy-MM-dd}-{++counter}.log");
                if (!file.Exists && file.Directory != null)
                {
                    file.Directory.Create();
                    break;
                }
            }

            Logger.SetLogFile(file);
            // Starting the server
            logger.Info("Starting DanhengServer...");

            // Load the config
            logger.Info("Loading config...");
            try
            {
                ConfigManager.LoadConfig();
            } catch (Exception e)
            {
                logger.Error("Failed to load config", e);
                Console.ReadLine();
                return;
            }

            // Load the language
            logger.Info("Loading language...");
            try
            {
                I18nManager.LoadLanguage();
            } catch (Exception e)
            {
                logger.Error("Failed to load language", e);
                Console.ReadLine();
                return;
            }

            // Load the game data
            logger.Info("Loading game data...");
            try
            {
                ResourceManager.LoadGameData();
            } catch (Exception e)
            {
                logger.Error("Failed to load game data", e);
                Console.ReadLine();
                return;
            }

            // Initialize the database
            try
            {
                DatabaseHelper.Initialize();

                if (args.Contains("--upgrade-database"))
                {
                    DatabaseHelper.UpgradeDatabase();
                }

                if (args.Contains("--move"))
                {
                    DatabaseHelper.MoveFromSqlite();
                }
            } catch (Exception e)
            {
                logger.Error("Failed to initialize database", e);
                Console.ReadLine();
                return;
            }

            // Register the command handlers
            try
            {
                CommandManager.RegisterCommand();
            } catch (Exception e)
            {
                logger.Error("Failed to initialize command manager", e);
                Console.ReadLine();
                return;
            }

            // Load the plugins
            logger.Info("Loading plugins...");
            try
            {
                PluginManager.LoadPlugins();
            }
            catch (Exception e)
            {
                logger.Error("Failed to load plugins", e);
                Console.ReadLine();
                return;
            }

            CommandExecutor.OnRunCommand += (sender, e) =>
            {
                CommandManager.HandleCommand(e, sender);
            };

            MuipManager.OnExecuteCommand += CommandManager.HandleCommand;
            MuipManager.OnGetServerInformation += x =>
            {
                foreach (var con in Listener.Connections.Values)
                {
                    if (con.Player != null)
                    {
                        x.Add(con.Player.Uid, con.Player.Data);
                    }
                }
            };
            MuipManager.OnGetPlayerStatus += (int uid, out PlayerStatusEnum status) =>
            {
                foreach (var con in Listener.Connections.Values)
                {
                    if (con.Player != null && con.Player.Uid == uid)
                    {
                        if (con.Player.RogueManager?.GetRogueInstance() != null)
                        {
                            status = PlayerStatusEnum.Rogue;
                        }
                        else if (con.Player.ChallengeManager?.ChallengeInstance != null)
                        {
                            status = PlayerStatusEnum.Challenge;
                        }
                        else
                        {
                            status = PlayerStatusEnum.Explore;
                        }
                        return;
                    }
                }

                status = PlayerStatusEnum.Offline;
            };

            // generate the handbook
            HandbookGenerator.Generate();

            HandlerManager.Init();

            WebProgram.Main([], GetConfig().HttpServer.PublicPort, GetConfig().HttpServer.GetDisplayAddress());
            logger.Info($"Dispatch Server is running on {GetConfig().HttpServer.GetDisplayAddress()}");

            Listener.StartListener();

            var elapsed = DateTime.Now - time;
            logger.Info($"Done in {elapsed.TotalSeconds.ToString()[..4]}s! Type '/help' to get help of commands.");

            GenerateLogMap();

            if (GetConfig().ServerOption.EnableMission)
            {
                logger.Warn("Mission system is enabled. This is a feature that is still in development and may not work as expected. If you encounter any issues, please report them to the developers.");
            }
            CommandManager.Start();
        }

        public static ConfigContainer GetConfig()
        {
            return ConfigManager.Config;
        }

        private static void PerformCleanup()
        {
            PluginManager.UnloadPlugins();
            Listener.Connections.Values.ToList().ForEach(x => x.Stop());

            DatabaseHelper.SaveThread?.Interrupt();
            DatabaseHelper.SaveDatabase();
        }

        private static void GenerateLogMap()
        {
            // get opcode from CmdIds
            var opcodes = typeof(CmdIds).GetFields().Where(x => x.FieldType == typeof(int)).ToList();
            foreach (var opcode in opcodes)
            {
                var name = opcode.Name;
                var value = (int)opcode.GetValue(null)!;
                Connection.LogMap.Add(value.ToString(), name);
            }
        }
    }
}
