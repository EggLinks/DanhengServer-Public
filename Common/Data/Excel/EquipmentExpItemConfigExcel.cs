namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("EquipmentExpItemConfig.json")]
public class EquipmentExpItemConfigExcel : ExcelResource
{
    public int ItemID { get; set; }
    public int ExpProvide { get; set; }

    public override int GetId()
    {
        return ItemID;
    }

    public override void Loaded()
    {
        if (ExpProvide > 0) GameData.EquipmentExpItemConfigData.Add(GetId(), this);
    }

    public override void AfterAllDone()
    {
        GameData.ItemConfigData.TryGetValue(ItemID, out var itemConfig);
        if (itemConfig == null) return;
        itemConfig.Exp = ExpProvide;
    }
}