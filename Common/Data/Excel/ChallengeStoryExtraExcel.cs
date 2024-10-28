namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ChallengeStoryMazeExtra.json")]
public class ChallengeStoryExtraExcel : ExcelResource
{
    public int ID { get; set; }
    public int TurnLimit { get; set; }
    public int ClearScore { get; set; }
    public List<int>? BattleTargetID { get; set; }


    public override int GetId()
    {
        return ID;
    }

    public override void AfterAllDone()
    {
        if (GameData.ChallengeConfigData.ContainsKey(ID))
        {
            var challengeExcel = GameData.ChallengeConfigData[ID];
            challengeExcel.SetStoryExcel(this);
        }
    }
}