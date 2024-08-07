using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Enums.Quest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("FinishWay.json,FinishWayRogue.json,FinishWayEventMission.json,RogueDLCFinishWay.json", true)]
public class FinishWayExcel : ExcelResource
{
    public List<int> ParamIntList { get; set; } = [];
    public string ParamStr1 { get; set; } = "";
    public List<MappingInfoItem> ParamItemList { get; set; } = [];
    public int MazeFloorID { get; set; }
    public int Progress { get; set; }
    public int ID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public QuestParaTypeEnum ParamType { get; set; }

    public int ParamInt1 { get; set; }
    public int ParamInt2 { get; set; }
    public int MazeDimensionID { get; set; }
    public int MazePlaneID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public MissionFinishTypeEnum FinishType { get; set; }

    public int ParamInt3 { get; set; }
    public bool IsBackTrack { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.FinishWayData.TryAdd(ID, this);
    }
}