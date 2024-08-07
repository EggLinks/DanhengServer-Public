using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Challenge;

[SugarTable("Challenge")]
public class ChallengeData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public Dictionary<int, ChallengeHistoryData> History { get; set; } = new();

    [SugarColumn(IsJson = true)] public ChallengeInstanceData Instance { get; set; } = new();

    [SugarColumn(IsJson = true)] public Dictionary<int, ChallengeGroupReward> TakenRewards { get; set; } = new();

    public void delete(int ChallengeId)
    {
        History.Remove(ChallengeId);
    }
}

public class ChallengeHistoryData(int uid, int challengeId)
{
    public int OwnerId { get; set; } = uid;

    public int ChallengeId { get; set; } = challengeId;
    public int GroupId { get; set; }
    public int TakenReward { get; set; }
    public int Stars { get; set; }
    public int Score { get; set; }

    public void SetStars(int stars)
    {
        Stars = Math.Max(Stars, stars);
    }

    public int GetTotalStars()
    {
        var total = 0;
        for (var i = 0; i < 3; i++) total += (Stars & (1 << i)) != 0 ? 1 : 0;
        return total;
    }

    public Proto.Challenge ToProto()
    {
        var proto = new Proto.Challenge
        {
            ChallengeId = (uint)ChallengeId,
            TakenReward = (uint)TakenReward,
            ScoreId = (uint)Score,
            Star = (uint)Stars
        };

        return proto;
    }
}

public class ChallengeInstanceData
{
    public Position StartPos { get; set; } = new();
    public Position StartRot { get; set; } = new();
    public int ChallengeId { get; set; } = 0;
    public int CurrentStage { get; set; }
    public int CurrentExtraLineup { get; set; }
    public int Status { get; set; }
    public bool HasAvatarDied { get; set; }

    public int SavedMp { get; set; }
    public int RoundsLeft { get; set; }
    public int Stars { get; set; }
    public int ScoreStage1 { get; set; }
    public int ScoreStage2 { get; set; }
    public List<int> StoryBuffs { get; set; } = [];
    public List<int> BossBuffs { get; set; } = [];
}

public class ChallengeGroupReward(int uid, int groupId)
{
    public int OwnerUid = uid;
    public int GroupId { get; set; } = groupId;
    public long TakenStars { get; set; }

    public bool HasTakenReward(int starCount)
    {
        return (TakenStars & (1L << starCount)) != 0;
    }

    public void SetTakenReward(int starCount)
    {
        TakenStars |= 1L << starCount;
    }

    public ChallengeGroup ToProto()
    {
        var proto = new ChallengeGroup
        {
            GroupId = (uint)GroupId,
            TakenStarsCountReward = (ulong)TakenStars
        };

        return proto;
    }
}