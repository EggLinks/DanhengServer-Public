namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueHandBookEvent.json")]
public class RogueHandBookEventExcel : ExcelResource
{
    public int EventHandbookID { get; set; }
    public HashName EventTitle { get; set; } = new();

    public int EventReward { get; set; }
    public List<int> EventTypeList { get; set; } = [];

    public override int GetId()
    {
        return EventHandbookID;
    }

    public override void Loaded()
    {
        GameData.RogueHandBookEventData.Add(GetId(), this);
    }
}