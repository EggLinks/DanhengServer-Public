using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("ShopGoodsConfig.json")]
    public class ShopGoodsConfigExcel : ExcelResource
    {
        public int GoodsID { get; set; }
        public int ShopID { get; set; }
        public int ItemID { get; set; }
        public int ItemCount { get; set; }
        public List<int> CurrencyList { get; set; } = [];
        public List<int> CurrencyCostList { get; set; } = [];

        [JsonIgnore]
        public Dictionary<int, int> CostList { get; set; } = [];

        public override int GetId()
        {
            return GoodsID;
        }

        public override void Loaded()
        {
            for (int i = 0; i < CurrencyList.Count; i++)
            {
                CostList.Add(CurrencyList[i], CurrencyCostList[i]);
            }
        }

        public override void AfterAllDone()
        {
            var shopConfig = GameData.ShopConfigData[ShopID];
            shopConfig.Goods.Add(this);
        }

        public Goods ToProto() => new()
        {
            EndTime = long.MaxValue,
            GoodsId = (uint)GoodsID,
            ItemId = (uint)ItemID,
        };
    }
}
