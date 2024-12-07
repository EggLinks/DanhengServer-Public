namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournDifficulty.json")]
public class RogueTournDifficultyExcel : ExcelResource
{
    public int DifficultyID { get; set; }
    public List<int> DifficultyCutList { get; set; } = [];
    public List<int> DifficultyLevelGroup { get; set; } = [];
    public List<int> LevelList { get; set; } = [];

    public override int GetId()
    {
        return DifficultyID;
    }

    public override void Loaded()
    {
        GameData.RogueTournDifficultyData.Add(GetId(), this);
    }
}