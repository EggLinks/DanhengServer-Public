namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TrainPartyStepConfig.json")]
public class TrainPartyStepConfigExcel : ExcelResource
{
    public int ID { get; set; }
    public int CoinCost { get; set; }
    public int GroupID { get; set; }
    public int SortID { get; set; }
    public List<int> StaticPropIDList { get; set; } = [];

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.TrainPartyStepConfigData.Add(ID, this);
    }
}