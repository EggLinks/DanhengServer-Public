namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MapEntrance.json", true)]
public class MapEntranceExcel : ExcelResource
{
    public int ID { get; set; }
    public int PlaneID { get; set; }
    public int FloorID { get; set; }
    public int StartGroupID { get; set; }
    public int StartAnchorID { get; set; }

    public List<int> FinishMainMissionList { get; set; } = [];
    public List<int> FinishSubMissionList { get; set; } = [];

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MapEntranceData.Add(ID, this);
    }
}