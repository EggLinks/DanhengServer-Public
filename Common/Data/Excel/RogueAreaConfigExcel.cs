using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueAreaConfig.json")]
public class RogueAreaConfigExcel : ExcelResource
{
    public int RogueAreaID { get; set; }
    public int AreaProgress { get; set; }
    public int Difficulty { get; set; }
    public int FirstReward { get; set; }
    public Dictionary<int, int> ScoreMap { get; set; } = [];

    [JsonIgnore] public int MapId { get; set; }

    [JsonIgnore] public Dictionary<int, RogueMapExcel> RogueMaps { get; set; } = [];

    public override int GetId()
    {
        return RogueAreaID;
    }

    public override void Loaded()
    {
        GameData.RogueAreaConfigData.Add(RogueAreaID, this);

        MapId = AreaProgress * 100 + Difficulty;
    }

    public override void AfterAllDone()
    {
        GameData.RogueMapData.TryGetValue(MapId, out var map);
        if (map != null) RogueMaps = map;
    }
}