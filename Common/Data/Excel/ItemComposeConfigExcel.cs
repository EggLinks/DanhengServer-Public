namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ItemComposeConfig.json")]
public class ItemComposeConfigExcel : ExcelResource
{
    public int ID { get; set; }
    public int ItemID { get; set; }
    public int CoinCost { get; set; }
    public List<MaterialItem> MaterialCost { get; set; } = [];

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.ItemComposeConfigData[ID] = this;
    }
}

public class MaterialItem
{
    public int ItemID { get; set; }
    public int ItemNum { get; set; }
}