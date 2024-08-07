namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ItemUseBuffData.json")]
public class ItemUseBuffDataExcel : ExcelResource
{
    public int UseDataID { get; set; }
    public float PreviewSkillPoint { get; set; }
    public float PreviewHPRecoveryPercent { get; set; }
    public float PreviewHPRecoveryValue { get; set; }
    public int MazeBuffID { get; set; }
    public int MazeBuffID2 { get; set; }
    public float PreviewPowerPercent { get; set; }

    public override int GetId()
    {
        return UseDataID;
    }

    public override void Loaded()
    {
        GameData.ItemUseBuffDataData.TryAdd(UseDataID, this);
    }
}