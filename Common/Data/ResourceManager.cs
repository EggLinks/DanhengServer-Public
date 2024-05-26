using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using EggLink.DanhengServer.Data.Config;
using System.Xml.Linq;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EggLink.DanhengServer.Data.Custom;

namespace EggLink.DanhengServer.Data
{
    public class ResourceManager
    {
        public static Logger Logger { get; private set; } = new Logger("ResourceManager");
        public static void LoadGameData()
        {
            LoadExcel();
            LoadFloorInfo();
            LoadMissionInfo();
            LoadMazeSkill();
            LoadDialogueInfo();
            GameData.ActivityConfig = LoadCustomFile<ActivityConfig>("Activity", "ActivityConfig") ?? new();
            GameData.BannersConfig = LoadCustomFile<BannersConfig>("Banner", "Banners") ?? new();
            GameData.RogueMapGenData = LoadCustomFile<Dictionary<int, List<int>>>("Rogue Map", "RogueMapGen") ?? [];
            GameData.RogueMiracleGroupData = LoadCustomFile<Dictionary<int, List<int>>>("Rogue Miracle Group", "RogueMiracleGroup") ?? [];
            GameData.RogueMiracleEffectData = LoadCustomFile<RogueMiracleEffectConfig>("Rogue Miracle Effect", "RogueMiracleEffectGen") ?? new();
            GameData.ChessRogueLayerGenData = LoadCustomFile<Dictionary<int, Dictionary<int, List<int>>>>("Chess Rogue Layer", "ChessRogueLayerGen") ?? [];
            GameData.ChessRogueRoomGenData = LoadCustomFile<Dictionary<int, ChessRogueRoomConfig>>("Chess Rogue Map", "ChessRogueMapGen") ?? [];
            GameData.ChessRogueContentGenData = LoadCustomFile<Dictionary<int, List<int>>>("Chess Rogue Content", "ChessRogueContentGen") ?? [];
            GameData.ChessRogueCellGenData = LoadCustomFile<Dictionary<int, ChessRogueCellConfig>>("Chess Rogue Cell", "ChessRogueRoomGen") ?? [];
        }

        public static void LoadExcel()
        {

            var classes = Assembly.GetExecutingAssembly().GetTypes();  // Get all classes in the assembly
            var resList = new List<ExcelResource>();
            foreach (var cls in classes)
            {
                var attribute = (ResourceEntity)Attribute.GetCustomAttribute(cls, typeof(ResourceEntity))!;

                if (attribute != null)
                {
                    var resource = (ExcelResource)Activator.CreateInstance(cls)!;
                    var count = 0;
                    foreach (var fileName in attribute.FileName)
                    {
                        try
                        {
                            var path = ConfigManager.Config.Path.ResourcePath + "/ExcelOutput/" + fileName;
                            var file = new FileInfo(path);
                            if (!file.Exists)
                            {
                                Logger.Warn($"File {path} not found");
                                continue;
                            }
                            var json = file.OpenText().ReadToEnd();
                            using (var reader = new JsonTextReader(new StringReader(json)))
                            {
                                reader.Read();
                                if (reader.TokenType == JsonToken.StartArray)
                                {
                                    // array
                                    var jArray = JArray.Parse(json);
                                    foreach (var item in jArray)
                                    {
                                        var res = JsonConvert.DeserializeObject(item.ToString(), cls);
                                        resList.Add((ExcelResource)res!);
                                        ((ExcelResource?)res)?.Loaded();
                                        count++;
                                    }
                                }
                                else if (reader.TokenType == JsonToken.StartObject)
                                {
                                    // dictionary
                                    var jObject = JObject.Parse(json);
                                    foreach (var item in jObject)
                                    {
                                        var id = int.Parse(item.Key);
                                        var obj = item.Value;
                                        var instance = JsonConvert.DeserializeObject(obj!.ToString(), cls);

                                        if (((ExcelResource?)instance)?.GetId() == 0 || ((ExcelResource?)instance) == null)
                                        {
                                            // Deserialize as JObject to handle nested dictionaries
                                            var nestedObject = JsonConvert.DeserializeObject<JObject>(obj.ToString());

                                            foreach (var nestedItem in nestedObject ?? [])
                                            {
                                                var nestedInstance = JsonConvert.DeserializeObject(nestedItem.Value!.ToString(), cls);
                                                resList.Add((ExcelResource)nestedInstance!);
                                                ((ExcelResource?)nestedInstance)?.Loaded();
                                                count++;
                                            }
                                        }
                                        else
                                        {
                                            resList.Add((ExcelResource)instance!);
                                            ((ExcelResource)instance).Loaded();
                                        }
                                        count++;
                                    }
                                }
                            }
                            resource.Finalized();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error in reading {fileName}", ex);
                        }
                    }
                    Logger.Info($"Loaded {count} {cls.Name}s.");
                }
            }
            foreach (var cls in resList)
            {
                cls.AfterAllDone();
            }
        }

        public static void LoadFloorInfo()
        {
            Logger.Info("Loading floor files...");
            DirectoryInfo directory = new(ConfigManager.Config.Path.ResourcePath + "/Config/LevelOutput/Floor/");
            bool missingGroupInfos = false;

            if (!directory.Exists)
            {
                Logger.Warn($"Floor infos are missing, please check your resources folder: {ConfigManager.Config.Path.ResourcePath}/Config/LevelOutput/Floor. Teleports and natural world spawns may not work!");
                return;
            }
            // Load floor infos
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    using var reader = file.OpenRead();
                    using StreamReader reader2 = new(reader);
                    var text = reader2.ReadToEnd();
                    var info = JsonConvert.DeserializeObject<FloorInfo>(text);
                    var name = file.Name[..file.Name.IndexOf('.')];
                    GameData.FloorInfoData.Add(name, info!);
                } catch (Exception ex)
                {
                    Logger.Error("Error in reading" + file.Name, ex);
                }
            }

            foreach (var info in GameData.FloorInfoData.Values)
            {
                foreach (var groupInfo in info.GroupList)
                {
                    if (groupInfo.IsDelete) { continue; }
                    FileInfo file = new(ConfigManager.Config.Path.ResourcePath + "/" + groupInfo.GroupPath);
                    if (!file.Exists) continue;
                    try
                    {
                        using var reader = file.OpenRead();
                        using StreamReader reader2 = new(reader);
                        var text = reader2.ReadToEnd();
                        GroupInfo? group = JsonConvert.DeserializeObject<GroupInfo>(text);
                        if (group != null)
                        {
                            group.Id = groupInfo.ID;
                            info.Groups.Add(groupInfo.ID, group);
                            group.Load();
                        }
                    } catch (Exception ex)
                    {
                        Logger.Error("Error in reading " + file.Name, ex);
                    }
                    if (info.Groups.Count == 0)
                    {
                        missingGroupInfos = true;
                    }
                }
                info.OnLoad();
            }
            if (missingGroupInfos)
                Logger.Warn($"Group infos are missing, please check your resources folder: {ConfigManager.Config.Path.ResourcePath}/Config/LevelOutput/Group. Teleports, monster battles, and natural world spawns may not work!");

            Logger.Info("Loaded " + GameData.FloorInfoData.Count + " floor infos.");
        }

        public static void LoadMissionInfo()
        {
            Logger.Info("Loading mission files...");
            DirectoryInfo directory = new(ConfigManager.Config.Path.ResourcePath + "/Config/Level/Mission");
            if (!directory.Exists)
            {
                Logger.Warn($"Mission infos are missing, please check your resources folder: {ConfigManager.Config.Path.ResourcePath}/Config/Level/Mission. Missions may not work!");
                return;
            }
            bool missingMissionInfos = false;
            var count = 0;
            foreach (var missionExcel in GameData.MainMissionData)
            {
                var path = $"{ConfigManager.Config.Path.ResourcePath}/Config/Level/Mission/{missionExcel.Key}/MissionInfo_{missionExcel.Key}.json";
                if (!File.Exists(path))
                {
                    missingMissionInfos = true;
                    continue;
                }
                var json = File.ReadAllText(path);
                var missionInfo = JsonConvert.DeserializeObject<MissionInfo>(json);
                if (missionInfo != null)
                {
                    GameData.MainMissionData[missionExcel.Key].MissionInfo = missionInfo;
                    foreach (var subMission in missionInfo.SubMissionList)
                    {   // load mission json
                        var missionJsonPath = ConfigManager.Config.Path.ResourcePath + "/" + subMission.MissionJsonPath;
                        if (File.Exists(missionJsonPath))
                        {
                            var missionJson = File.ReadAllText(missionJsonPath).Replace("$type", "Type");
                            try
                            {
                                if (subMission.FinishType == Enums.MissionFinishTypeEnum.EnterFloor)
                                {
                                    var mission = JsonConvert.DeserializeObject<SubMissionTask<EnterFloorTaskInfo>>(missionJson);

                                    if (mission != null)
                                    {
                                        subMission.Task = mission;
                                        subMission.Loaded(1);
                                    }
                                } else if (subMission.FinishType == Enums.MissionFinishTypeEnum.PropState)
                                {
                                    var mission = JsonConvert.DeserializeObject<SubMissionTask<PropStateTaskInfo>>(missionJson);
                                    if (mission != null)
                                    {
                                        subMission.PropTask = mission;
                                        subMission.Loaded(2);
                                    }
                                } else if (subMission.FinishType == Enums.MissionFinishTypeEnum.StageWin)
                                {
                                    var mission = JsonConvert.DeserializeObject<SubMissionTask<StageWinTaskInfo>>(missionJson);
                                    if (mission != null)
                                    {
                                        subMission.StageWinTask = mission;
                                        subMission.Loaded(3);
                                    }
                                }
                                else
                                {
                                    subMission.Loaded(0);
                                }
                            } catch (Exception ex)
                            {
                                Logger.Error("Error in reading " + missionJsonPath, ex);
                            }
                        }
                    }
                    count++;
                } else
                {
                    missingMissionInfos = true;
                }
            }
            if (missingMissionInfos)
                Logger.Warn($"Mission infos are missing, please check your resources folder: {ConfigManager.Config.Path.ResourcePath}/Config/Level/Mission. Missions may not work!");
            Logger.Info("Loaded " + count + " mission infos.");
        }

        public static T? LoadCustomFile<T>(string filetype, string filename)
        {
            Logger.Info($"Loading {filetype} files...");
            FileInfo file = new(ConfigManager.Config.Path.ConfigPath + $"/{filename}.json");
            T? customFile = default;
            if (!file.Exists)
            {
                Logger.Warn($"Banner infos are missing, please check your resources folder: {ConfigManager.Config.Path.ConfigPath}/{filename}.json. {filetype} may not work!");
                return customFile;
            }
            try
            {
                using var reader = file.OpenRead();
                using StreamReader reader2 = new(reader);
                var text = reader2.ReadToEnd();
                var json = JsonConvert.DeserializeObject<T>(text);
                customFile = json;
            } catch (Exception ex)
            {
                Logger.Error("Error in reading " + file.Name, ex);
            }

            if (customFile is Dictionary<int, int> d)
            {
                Logger.Info("Loaded " + d.Count + $" {filetype}s.");
            } else if (customFile is Dictionary<int, List<int>> di)
            {
                Logger.Info("Loaded " + di.Count + $" {filetype}s.");
            } else if (customFile is BannersConfig c)
            {
                Logger.Info("Loaded " + c.Banners.Count + $" {filetype}s.");
            } else if (customFile is RogueMiracleEffectConfig r)
            {
                Logger.Info("Loaded " + r.Miracles.Count + $" {filetype}s.");
            } else if (customFile is ActivityConfig a)
            {
                Logger.Info("Loaded " + a.ScheduleData.Count + $" {filetype}s.");
            }
            else
            {
                Logger.Info("Loaded " + filetype + " file.");
            }

            return customFile;
        }

        public static void LoadMazeSkill()
        {
            var count = 0;
            foreach (var avatar in GameData.AvatarConfigData.Values)
            {
                var path = ConfigManager.Config.Path.ResourcePath + "/Config/ConfigAdventureAbility/LocalPlayer/LocalPlayer_" + avatar.NameKey + "_Ability.json";
                var file = new FileInfo(path);
                if (!file.Exists) continue;
                try
                {
                    using var reader = file.OpenRead();
                    using StreamReader reader2 = new(reader);
                    var text = reader2.ReadToEnd().Replace("$type", "Type");
                    var skillAbilityInfo = JsonConvert.DeserializeObject<SkillAbilityInfo>(text);
                    skillAbilityInfo?.Loaded(avatar);
                    count += skillAbilityInfo == null ? 0 : 1;
                } catch (Exception ex)
                {
                    Logger.Error("Error in reading " + file.Name, ex);
                }
            }
            if (count < GameData.AvatarConfigData.Count)
            {
                Logger.Warn("Maze skill infos are missing, please check your resources folder: " + ConfigManager.Config.Path.ResourcePath + "/Config/ConfigAdventureAbility/LocalPlayer. Maze skills may not work!");
            }
            Logger.Info("Loaded " + count + " maze skill infos.");
        }

        public static void LoadDialogueInfo()
        {
            var count = 0;
            foreach (var dialogue in GameData.RogueNPCDialogueData)
            {
                var path = ConfigManager.Config.Path.ResourcePath + "/" + dialogue.Value.DialoguePath;
                var file = new FileInfo(path);
                if (!file.Exists) continue;
                try
                {
                    using var reader = file.OpenRead();
                    using StreamReader reader2 = new(reader);
                    var text = reader2.ReadToEnd().Replace("$type", "Type");
                    var dialogueInfo = JsonConvert.DeserializeObject<DialogueInfo>(text);
                    if (dialogueInfo != null)
                    {
                        dialogue.Value.DialogueInfo = dialogueInfo;
                        dialogueInfo.Loaded();
                        if (dialogueInfo.DialogueIds.Count == 0)
                        {
                            // set to invalid
                            dialogue.Value.DialogueInfo = null;
                        }
                        count++;
                    }
                } catch (Exception ex)
                {
                    Logger.Error("Error in reading " + file.Name, ex);
                }

            }

            if (count < GameData.RogueNPCDialogueData.Count)
            {
                Logger.Warn("Dialogue infos are missing, please check your resources folder: " + ConfigManager.Config.Path.ResourcePath + "/Config/Level/RogueDialogue/RogueDialogueEvent. Dialogues may not work!");
            }

            Logger.Info("Loaded " + count + " dialogue infos.");
        }
    }
}
