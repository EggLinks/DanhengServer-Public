using EggLink.DanhengServer.Enums.TournRogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournArea.json")]
public class RogueTournAreaExcel : ExcelResource
{
    public List<int> MonsterDisplayItemList { get; set; } = [];
    public List<int> LayerIDList { get; set; } = [];
    public List<int> DifficultyIDList { get; set; } = [];
    public int WorldLevelLimit { get; set; }
    public int FirstReward { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTournDifficultyTypeEnum Difficulty { get; set; }

    public int ExpScoreID { get; set; }
    public int UnlockID { get; set; }
    public int AreaID { get; set; }
    public HashName AreaNameID { get; set; } = new();
    public bool IsHard { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTournModeEnum TournMode { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTournAreaGroupIDEnum AreaGroupID { get; set; }

    public override int GetId()
    {
        return AreaID;
    }

    public override void Loaded()
    {
        GameData.RogueTournAreaData.TryAdd(AreaID, this);
    }
}