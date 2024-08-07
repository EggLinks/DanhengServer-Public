using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ShopGoodsConfig.json")]
public class ShopGoodsConfigExcel : ExcelResource
{
    public int GoodsID { get; set; }
    public int ShopID { get; set; }
    public int ItemID { get; set; }
    public int ItemCount { get; set; }
    public List<int> CurrencyList { get; set; } = [];
    public List<int> CurrencyCostList { get; set; } = [];

    [JsonIgnore] public Dictionary<int, int> CostList { get; set; } = [];

    public override int GetId()
    {
        return GoodsID;
    }

    public override void Loaded()
    {
        for (var i = 0; i < CurrencyList.Count; i++) CostList.Add(CurrencyList[i], CurrencyCostList[i]);
    }

    public override void AfterAllDone()
    {
        var shopConfig = GameData.ShopConfigData[ShopID];
        shopConfig.Goods.Add(this);
    }

    public Goods ToProto()
    {
        return new Goods
        {
            EndTime = uint.MaxValue,
            GoodsId = (uint)GoodsID,
            ItemId = (uint)ItemID
        };
    }
}