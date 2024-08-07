using EggLink.DanhengServer.Enums.Mission;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("StoryLine.json")]
public class StoryLineExcel : ExcelResource
{
    public int StoryLineID { get; set; }
    public StoryLineCondition BeginCondition { get; set; } = new();
    public StoryLineCondition EndCondition { get; set; } = new();
    public int InitEntranceID { get; set; }
    public int InitGroupID { get; set; }
    public int InitAnchorID { get; set; }

    public override int GetId()
    {
        return StoryLineID;
    }

    public override void Loaded()
    {
        GameData.StoryLineData[StoryLineID] = this;
    }
}

public class StoryLineCondition
{
    [JsonConverter(typeof(StringEnumConverter))]
    public StoryLineConditionTypeEnum Type { get; set; } = new();

    public int Param { get; set; }
}