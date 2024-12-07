namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMagicDifficultyComp.json")]
public class RogueMagicDifficultyCompExcel : ExcelResource
{
    public int DifficultyCompID { get; set; }
    public int Level { get; set; }
    public int UnlockID { get; set; }

    public override int GetId()
    {
        return DifficultyCompID;
    }

    public override void Loaded()
    {
        GameData.RogueMagicDifficultyCompData.Add(DifficultyCompID, this);
    }
}