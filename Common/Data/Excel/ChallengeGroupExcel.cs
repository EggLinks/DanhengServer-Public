namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ChallengeGroupConfig.json,ChallengeStoryGroupConfig.json,ChallengeBossGroupConfig.json",
    true)]
public class ChallengeGroupExcel : ExcelResource
{
    public int GroupID { get; set; }
    public int RewardLineGroupID { get; set; }
    public int SchduleID { get; set; }

    public override int GetId()
    {
        return GroupID;
    }

    public override void Loaded()
    {
        GameData.ChallengeGroupData[GroupID] = this;
    }
}