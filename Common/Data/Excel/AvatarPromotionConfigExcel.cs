namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("AvatarPromotionConfig.json")]
public class AvatarPromotionConfigExcel : ExcelResource
{
    public int AvatarID { get; set; }
    public int Promotion { get; set; }
    public int MaxLevel { get; set; }
    public int PlayerLevelRequire { get; set; }
    public int WorldLevelRequire { get; set; }
    public List<ItemParam> PromotionCostList { get; set; } = [];

    public override int GetId()
    {
        return AvatarID * 10 + Promotion;
    }

    public override void Loaded()
    {
        GameData.AvatarPromotionConfigData.Add(GetId(), this);
    }

    public class ItemParam
    {
        public int ItemID;
        public int ItemNum;
    }
}