using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Config;

public class MissionInfo
{
    public int MainMissionID { get; set; }
    public List<int> StartSubMissionList { get; set; } = [];
    public List<int> FinishSubMissionList { get; set; } = [];
    public List<SubMissionInfo> SubMissionList { get; set; } = [];
    public List<CustomValueInfo> MissionCustomValueList { get; set; } = [];
}

public class SubMissionInfo
{
    public int ID { get; set; }
    public int LevelPlaneID { get; set; }
    public int LevelFloorID { get; set; }
    public int MainMissionID { get; set; }
    public string MissionJsonPath { get; set; } = "";

    [JsonConverter(typeof(StringEnumConverter))]
    public SubMissionTakeTypeEnum TakeType { get; set; }

    public List<int>? TakeParamIntList { get; set; } = []; // the mission's prerequisites

    [JsonConverter(typeof(StringEnumConverter))]
    public MissionFinishTypeEnum FinishType { get; set; }

    public int ParamInt1 { get; set; }
    public int ParamInt2 { get; set; }
    public int ParamInt3 { get; set; }
    public string ParamStr1 { get; set; } = "";
    public List<int>? ParamIntList { get; set; } = [];
    public List<MaterialItem>? ParamItemList { get; set; } = [];
    public List<FinishActionInfo>? FinishActionList { get; set; } = [];
    public int Progress { get; set; }
    public List<int>? GroupIDList { get; set; } = [];
    public int SubRewardID { get; set; }
}

public class CustomValueInfo
{
    public int Index { get; set; }
    public List<int> ValidValueParamList { get; set; } = [];
}

public class FinishActionInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public FinishActionTypeEnum FinishActionType { get; set; }

    public List<int> FinishActionPara { get; set; } = [];
    public List<string> FinishActionParaString { get; set; } = [];
}