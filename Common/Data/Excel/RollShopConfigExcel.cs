namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RollShopConfig.json")]
public class RollShopConfigExcel : ExcelResource
{
    public int RollShopID { get; set; }
    public List<SpecialGroup> SpecialGroupList { get; set; } = [];
    public uint CostItemID { get; set; }
    public uint CostItemNum { get; set; }
    public uint T1GroupID { get; set; }
    public uint T2GroupID { get; set; }
    public uint T3GroupID { get; set; }
    public uint T4GroupID { get; set; }
    public uint SecretGroupID { get; set; }
    public string RollShopType { get; set; } = "";
    public uint IntroduceID { get; set; }
    public HashName ShopName { get; set; } = new();

    public override int GetId()
    {
        return RollShopID;
    }

    public override void Loaded()
    {
        GameData.RollShopConfigData.Add(GetId(), this);
    }
}

public class SpecialGroup
{
    public string GroupID { get; set; } = "";
    public int GroupValue { get; set; }
}