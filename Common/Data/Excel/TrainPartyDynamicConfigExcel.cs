namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TrainPartyDynamicConfig.json")]
public class TrainPartyDynamicConfigExcel : ExcelResource
{
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.TrainPartyDynamicConfigData.Add(ID, this);
    }
}