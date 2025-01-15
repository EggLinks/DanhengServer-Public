namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TrainPartyAreaGoalConfig.json")]
public class TrainPartyAreaGoalConfigExcel : ExcelResource
{
    public int AreaID { get; set; }
    public int ID { get; set; }
    public List<int> StepGroupList { get; set; } = [];

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.TrainPartyAreaGoalConfigData.Add(ID, this);
    }
}