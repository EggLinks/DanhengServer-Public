namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MatchThreeLevel.json")]
public class MatchThreeLevelExcel : ExcelResource
{
    public int Mode { get; set; }
    public int OpponentBirdID { get; set; }
    public int GoMissionCondition { get; set; }
    public int LevelID { get; set; }
    public int PlayerID { get; set; }
    public int LevelMission { get; set; }
    public int UnlockID { get; set; }
    public int TurnStep { get; set; }
    public int OpponentID { get; set; }
    public int PlayerBirdID { get; set; }
    public int RewardID { get; set; }

    public override int GetId()
    {
        return LevelID * 10 + Mode;
    }

    public override void Loaded()
    {
        GameData.MatchThreeLevelData.TryAdd(LevelID * 10 + Mode, this);
    }
}