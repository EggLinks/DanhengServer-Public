using EggLink.DanhengServer.Proto;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Quests;

[SugarTable("quest_data")]
public class QuestData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true, ColumnDataType = "MEDIUMTEXT")]
    public Dictionary<int, QuestInfo> Quests { get; set; } = [];
}

public class QuestInfo
{
    public int QuestId { get; set; }
    public QuestStatus QuestStatus { get; set; }
    public int Progress { get; set; }
    public long FinishTime { get; set; }

    public Quest ToProto()
    {
        return new Quest
        {
            Id = (uint)QuestId,
            Status = QuestStatus,
            Progress = (uint)Progress,
            FinishTime = FinishTime
        };
    }
}