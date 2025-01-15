namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TrainPartyPassengerConfig.json")]
public class TrainPartyPassengerConfigExcel : ExcelResource
{
    public int PassengerID { get; set; }
    public int PassengerQuest { get; set; }

    public override int GetId()
    {
        return PassengerID;
    }

    public override void Loaded()
    {
        GameData.TrainPartyPassengerConfigData.Add(PassengerID, this);
    }
}