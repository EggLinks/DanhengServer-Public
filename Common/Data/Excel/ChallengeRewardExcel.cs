namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ChallengeMazeRewardLine.json,ChallengeStoryRewardLine.json", true)]
public class ChallengeRewardExcel : ExcelResource
{
    public int GroupID { get; set; }
    public int StarCount { get; set; }
    public int RewardID { get; set; }

    public override int GetId()
    {
        return (GroupID << 16) + StarCount;
    }

    public override void Loaded()
    {
        if (!GameData.ChallengeRewardData.ContainsKey(GroupID))
            GameData.ChallengeRewardData[GroupID] = new List<ChallengeRewardExcel>();
        GameData.ChallengeRewardData[GroupID].Add(this);
    }
}