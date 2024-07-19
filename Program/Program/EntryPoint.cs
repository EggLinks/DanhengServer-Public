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
                logger.Info(I18nManager.Translate("Server.ServerInfo.Shutdown"));
                PerformCleanup();
            };
            Console.CancelKeyPress += (sender, eventArgs) => {
                logger.Info(I18nManager.Translate("Server.ServerInfo.CancelKeyPressed"));
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
            logger.Info(I18nManager.Translate("Server.ServerInfo.StartingServer"));

            // Load the config
            logger.Info(I18nManager.Translate("Server.ServerInfo.LoadingItem", I18nManager.Translate("Word.Config")));
            try
            {
                ConfigManager.LoadConfig();
            } catch (Exception e)
            {
                logger.Error(I18nManager.Translate("Server.ServerInfo.FailedToLoadItem", I18nManager.Translate("Word.Config")), e);
                Console.ReadLine();
                return;
            }

            // Load the language
            logger.Info(I18nManager.Translate("Server.ServerInfo.LoadingItem", I18nManager.Translate("Word.Language")));
            try
            {
                I18nManager.LoadLanguage();
            } catch (Exception e)
            {
                logger.Error(I18nManager.Translate("Server.ServerInfo.FailedToLoadItem", I18nManager.Translate("Word.Language")), e);
                Console.ReadLine();
                return;
            }

            // Load the game data
            logger.Info(I18nManager.Translate("Server.ServerInfo.LoadingItem", I18nManager.Translate("Word.GameData")));
            try
            {
                ResourceManager.LoadGameData();
            } catch (Exception e)
            {
                logger.Error(I18nManager.Translate("Server.ServerInfo.FailedToLoadItem", I18nManager.Translate("Word.GameData")), e);
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
                logger.Error(I18nManager.Translate("Server.ServerInfo.FailedToLoadItem", I18nManager.Translate("Word.Database")), e);
                Console.ReadLine();
                return;
            }

            // Register the command handlers
            try
            {
                CommandManager.RegisterCommand();
            } catch (Exception e)
            {
                logger.Error(I18nManager.Translate("Server.ServerInfo.FailedToInitializeItem", I18nManager.Translate("Word.Command")), e);
                Console.ReadLine();
                return;
            }

            // Load the plugins
            logger.Info(I18nManager.Translate("Server.ServerInfo.LoadingItem", I18nManager.Translate("Word.Plugin")));
            try
            {
                PluginManager.LoadPlugins();
            }
            catch (Exception e)
            {
                logger.Error(I18nManager.Translate("Server.ServerInfo.FailedToLoadItem", I18nManager.Translate("Word.Plugin")), e);
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
            MuipManager.OnGetPlayerStatus += (int uid, out PlayerStatusEnum status, out PlayerSubStatusEnum subStatus) =>
            {
                subStatus = PlayerSubStatusEnum.None;
                foreach (var con in Listener.Connections.Values)
                {
                    if (con.Player != null && con.Player.Uid == uid)
                    {
                        if (con.Player.RogueManager?.GetRogueInstance() != null)
                        {
                            status = PlayerStatusEnum.Rogue;
                            if (con.Player.ChessRogueManager?.RogueInstance?.AreaExcel.RogueVersionId == 202)
                            {
                                status = PlayerStatusEnum.ChessRogueNous;
                            } 
                            else if (con.Player.ChessRogueManager?.RogueInstance?.AreaExcel.RogueVersionId == 201)
                            {
                                status = PlayerStatusEnum.ChessRogue;
                            }
                        }
                        else if (con.Player.ChallengeManager?.ChallengeInstance != null)
                        {
                            status = PlayerStatusEnum.Challenge;
                            if (con.Player.ChallengeManager.ChallengeInstance.Excel.StoryExcel != null)
                            {
                                status = PlayerStatusEnum.ChallengeStory;
                            }
                            else if (con.Player.ChallengeManager.ChallengeInstance.Excel.BossExcel != null)
                            {
                                status = PlayerStatusEnum.ChallengeBoss;
                            }
                        } 
                        else if (con.Player.RaidManager?.RaidData.CurRaidId != 0)
                        {
                            status = PlayerStatusEnum.Raid;
                        } 
                        else if (con.Player.StoryLineManager?.StoryLineData.CurStoryLineId != 0)
                        {
                            status = PlayerStatusEnum.StoryLine;
                        }
                        else
                        {
                            status = PlayerStatusEnum.Explore;
                        }

                        if (con.Player.BattleInstance != null)
                        {
                            subStatus = PlayerSubStatusEnum.Battle;
                        }

                        return;
                    }
                }

                status = PlayerStatusEnum.Offline;
            };

            // generate the handbook
            HandbookGenerator.Generate();

            HandlerManager.Init();

            WebProgram.Main([], GetConfig().HttpServer.BindPort, GetConfig().HttpServer.GetBindAddress());
            logger.Info(I18nManager.Translate("Server.ServerInfo.ServerRunning", I18nManager.Translate("Word.Dispatch"), GetConfig().HttpServer.GetDisplayAddress()));

            Listener.StartListener();

            var elapsed = DateTime.Now - time;
            logger.Info(I18nManager.Translate("Server.ServerInfo.ServerStarted", elapsed.TotalSeconds.ToString()[..4]));

            GenerateLogMap();

            if (GetConfig().ServerOption.EnableMission)
            {
                logger.Warn(I18nManager.Translate("Server.ServerInfo.MissionEnabled"));
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
