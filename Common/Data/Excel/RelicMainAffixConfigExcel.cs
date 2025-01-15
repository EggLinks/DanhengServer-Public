namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RelicMainAffixConfig.json")]
public class RelicMainAffixConfigExcel : ExcelResource
{
    public int GroupID { get; set; }
    public int AffixID { get; set; }

    public bool IsAvailable { get; set; }
    public string? Property { get; set; }

    public override int GetId()
    {
        return GroupID * 100 + AffixID;
    }

    public override void Loaded()
    {
        GameData.RelicMainAffixData.TryGetValue(GroupID, out var affixes);
        if (affixes != null)
            affixes[AffixID] = this;
        else
            GameData.RelicMainAffixData[GroupID] = new Dictionary<int, RelicMainAffixConfigExcel> { { AffixID, this } };
    }
}