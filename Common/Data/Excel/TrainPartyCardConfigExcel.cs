namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TrainPartyCardConfig.json")]
public class TrainPartyCardConfigExcel : ExcelResource
{
    public int CardID { get; set; }
    public int UpgradeLevel { get; set; }
    public int Rarity { get; set; }

    public override int GetId()
    {
        return CardID;
    }

    public override void Loaded()
    {
        GameData.TrainPartyCardConfigData[CardID] = this;
    }
}