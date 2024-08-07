using EggLink.DanhengServer.Proto;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Activity;

[SugarTable("Activity")]
public class ActivityData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public TrialActivityData TrialActivityData { get; set; } = new();
}

public class TrialActivityData
{
    public List<TrialActivityResultData> Activities { get; set; } = new();
    public int CurTrialStageId { get; set; } = 0;

    public List<TrialActivityInfo> ToProto()
    {
        var proto = new List<TrialActivityInfo>();

        foreach (var activity in Activities)
            proto.Add(new TrialActivityInfo
            {
                StageId = (uint)activity.StageId,
                TakenReward = activity.TakenReward
            });

        return proto;
    }
}

public class TrialActivityResultData
{
    public int StageId { get; set; } = 0;
    public bool TakenReward { get; set; } = false;
}