namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ChallengeTargetConfig.json,ChallengeStoryTargetConfig.json", true)]
public class ChallengeTargetExcel : ExcelResource
{
    public enum ChallengeType
    {
        None,
        ROUNDS,
        DEAD_AVATAR,
        KILL_MONSTER,
        AVATAR_BASE_TYPE_MORE,
        AVATAR_BASE_TYPE_LESS,
        ROUNDS_LEFT,
        TOTAL_SCORE
    }

    public int ID { get; set; }
    public ChallengeType ChallengeTargetType { get; set; }
    public int ChallengeTargetParam1 { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.ChallengeTargetData[ID] = this;
    }
}