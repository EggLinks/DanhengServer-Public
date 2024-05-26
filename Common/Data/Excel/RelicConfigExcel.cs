using EggLink.DanhengServer.Enums.Avatar;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RelicConfig.json")]
    public class RelicConfigExcel : ExcelResource
    {
        public int ID { get; set; }
        public int SetID { get; set; }

        public RelicTypeEnum Type { get; set; }
        public int MainAffixGroup { get; set; }
        public int SubAffixGroup { get; set; }
        public int MaxLevel { get; set; }
        public int ExpType { get; set; }

        public int ExpProvide { get; set; }
        public int CoinCost { get; set; }

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            GameData.RelicConfigData[ID] = this;
        }
    }
}
