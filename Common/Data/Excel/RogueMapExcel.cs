namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMap.json")]
public class RogueMapExcel : ExcelResource
{
    public int RogueMapID { get; set; }
    public int SiteID { get; set; }
    public bool IsStart { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }

    public List<int> NextSiteIDList { get; set; } = [];
    public List<int> LevelList { get; set; } = [];

    public override int GetId()
    {
        return RogueMapID * 1000 + SiteID;
    }

    public override void Loaded()
    {
        if (GameData.RogueMapData.TryGetValue(RogueMapID, out var map))
            map.Add(SiteID, this);
        else
            GameData.RogueMapData.Add(RogueMapID, new Dictionary<int, RogueMapExcel> { { SiteID, this } });
    }
}