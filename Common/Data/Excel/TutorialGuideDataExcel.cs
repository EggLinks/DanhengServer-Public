namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TutorialGuideData.json")]
public class TutorialGuideDataExcel : ExcelResource
{
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.TutorialGuideDataData.TryAdd(ID, this);
    }
}