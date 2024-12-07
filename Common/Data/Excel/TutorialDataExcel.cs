namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TutorialData.json")]
public class TutorialDataExcel : ExcelResource
{
    public int TutorialID { get; set; }
    public int Priority { get; set; }

    public override int GetId()
    {
        return TutorialID;
    }

    public override void Loaded()
    {
        GameData.TutorialDataData.TryAdd(TutorialID, this);
    }
}