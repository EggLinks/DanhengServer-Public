using EggLink.DanhengServer.Proto;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Message;

[SugarTable("Message")]
public class MessageData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public Dictionary<int, MessageGroupData> Groups { get; set; } = [];
}

public class MessageGroupData
{
    public int GroupId { get; set; }
    public List<MessageSectionData> Sections { get; set; } = [];
    public MessageGroupStatus Status { get; set; } = MessageGroupStatus.MessageGroupNone;
    public long RefreshTime { get; set; }
    public int CurrentSectionId { get; set; }
}

public class MessageSectionData
{
    public int SectionId { get; set; }
    public MessageSectionStatus Status { get; set; } = MessageSectionStatus.MessageSectionNone;
    public List<MessageItemData> Items { get; set; } = [];
    public List<int> ToChooseItemId { get; set; } = [];
}

public class MessageItemData
{
    public int ItemId { get; set; }
}