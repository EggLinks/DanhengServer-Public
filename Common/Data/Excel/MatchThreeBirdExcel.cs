namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MatchThreeBird.json")]
public class MatchThreeBirdExcel : ExcelResource
{
    public int UnlockLevel { get; set; }
    public int BirdID { get; set; }
    public int GuideID { get; set; }
    public int SkillID { get; set; }

    public override int GetId()
    {
        return BirdID;
    }

    public override void Loaded()
    {
        GameData.MatchThreeBirdData.TryAdd(BirdID, this);
    }
}