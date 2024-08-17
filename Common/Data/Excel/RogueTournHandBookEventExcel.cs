namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournHandBookEvent.json")]
public class RogueTournHandBookEventExcel : ExcelResource
{
    public int EventHandbookID { get; set; }

    public override int GetId()
    {
        return EventHandbookID;
    }

    public override void Loaded()
    {
        GameData.RogueTournHandBookEventData.TryAdd(GetId(), this);
    }
}