namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RelicSubAffixConfig.json")]
public class RelicSubAffixConfigExcel : ExcelResource
{
    public int GroupID { get; set; }
    public int AffixID { get; set; }

    public int StepNum { get; set; }
    public string? Property { get; set; }

    public override int GetId()
    {
        return GroupID * 100 + AffixID;
    }

    public override void Loaded()
    {
        GameData.RelicSubAffixData.TryGetValue(GroupID, out var affixes);
        if (affixes != null)
            affixes[AffixID] = this;
        else
            GameData.RelicSubAffixData[GroupID] = new Dictionary<int, RelicSubAffixConfigExcel> { { AffixID, this } };
    }
}