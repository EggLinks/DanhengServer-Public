namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournDifficultyComp.json")]
public class RogueTournDifficultyCompExcel : ExcelResource
{
    public int DifficultyCompID { get; set; }
    public int Level { get; set; }

    public override int GetId()
    {
        return DifficultyCompID;
    }

    public override void Loaded()
    {
        GameData.RogueTournDifficultyCompData.TryAdd(DifficultyCompID, this);
    }
}