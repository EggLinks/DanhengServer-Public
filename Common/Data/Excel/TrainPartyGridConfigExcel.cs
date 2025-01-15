namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TrainPartyGridConfig.json")]
public class TrainPartyGridConfigExcel : ExcelResource
{
    public int GridID { get; set; }
    public List<int> ParamList { get; set; } = [];

    public override int GetId()
    {
        return GridID;
    }

    public override void Loaded()
    {
        GameData.TrainPartyGridConfigData.Add(GridID, this);
    }
}