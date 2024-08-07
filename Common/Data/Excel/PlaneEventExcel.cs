namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("PlaneEvent.json")]
public class PlaneEventExcel : ExcelResource
{
    public int EventID { get; set; }
    public int WorldLevel { get; set; }
    public int Reward { get; set; }
    public List<int> DropList { get; set; } = [];
    public int StageID { get; set; }

    public override int GetId()
    {
        return EventID * 10 + WorldLevel;
    }

    public override void Loaded()
    {
        GameData.PlaneEventData.Add(GetId(), this);
    }
}