using EggLink.DanhengServer.Database.Quests;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Config.Scene;

public class GroupInfo
{
    public int Id;

    [JsonConverter(typeof(StringEnumConverter))]
    public GroupLoadSideEnum LoadSide { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public GroupCategoryEnum Category { get; set; }

    public LevelGroupSystemUnlockCondition? SystemUnlockCondition { get; set; } = null;
    public string LevelGraph { get; set; } = "";
    public bool LoadOnInitial { get; set; }
    public string GroupName { get; set; } = "";
    public LoadCondition LoadCondition { get; set; } = new();
    public LoadCondition UnloadCondition { get; set; } = new();
    public LoadCondition ForceUnloadCondition { get; set; } = new();

    [JsonConverter(typeof(StringEnumConverter))]
    public SaveTypeEnum SaveType { get; set; } = SaveTypeEnum.Save;

    public int OwnerMainMissionID { get; set; }
    public List<AnchorInfo> AnchorList { get; set; } = [];
    public List<MonsterInfo> MonsterList { get; set; } = [];
    public List<PropInfo> PropList { get; set; } = [];
    public List<NpcInfo> NPCList { get; set; } = [];

    [JsonIgnore] public LevelGraphConfigInfo? LevelGraphConfig { get; set; }

    [JsonIgnore] public Dictionary<string, List<int>> PropTriggerCustomString { get; set; } = [];

    public void Load()
    {
        foreach (var prop in PropList) prop.Load(this);
    }
}

public class LoadCondition
{
    public List<Condition> Conditions { get; set; } = [];

    [JsonConverter(typeof(StringEnumConverter))]
    public OperationEnum Operation { get; set; } = OperationEnum.And;

    public bool IsTrue(MissionData mission, bool defaultResult = true)
    {
        if (Conditions.Count == 0) return defaultResult;
        var canLoad = Operation == OperationEnum.And;
        // check load condition
        foreach (var condition in Conditions)
            if (condition.Type == LevelGroupMissionTypeEnum.MainMission)
            {
                var info = mission.GetMainMissionStatus(condition.ID);
                if (!ConfigManager.Config.ServerOption.EnableMission) info = MissionPhaseEnum.Finish;

                condition.Phase = condition.Phase == MissionPhaseEnum.Cancel
                    ? MissionPhaseEnum.Finish
                    : condition.Phase;

                if (info != condition.Phase)
                {
                    if (Operation == OperationEnum.And)
                    {
                        canLoad = false;
                        break;
                    }
                }
                else
                {
                    if (Operation == OperationEnum.Or)
                    {
                        canLoad = true;
                        break;
                    }
                }
            }
            else
            {
                // sub mission
                var status = mission.GetSubMissionStatus(condition.ID);
                if (!ConfigManager.Config.ServerOption.EnableMission) status = MissionPhaseEnum.Finish;
                condition.Phase = condition.Phase == MissionPhaseEnum.Cancel
                    ? MissionPhaseEnum.Finish
                    : condition.Phase;
                if (status != condition.Phase)
                {
                    if (Operation == OperationEnum.And)
                    {
                        canLoad = false;
                        break;
                    }
                }
                else
                {
                    if (Operation == OperationEnum.Or)
                    {
                        canLoad = true;
                        break;
                    }
                }
            }

        return canLoad;
    }
}

public class Condition
{
    [JsonConverter(typeof(StringEnumConverter))]
    public LevelGroupMissionTypeEnum Type { get; set; } = LevelGroupMissionTypeEnum.MainMission;

    public int ID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public MissionPhaseEnum Phase { get; set; } = MissionPhaseEnum.Accept;
}

public class LevelGroupSystemUnlockCondition
{
    public List<int> Conditions { get; set; } = [];

    [JsonConverter(typeof(StringEnumConverter))]
    public OperationEnum Operation { get; set; }
}