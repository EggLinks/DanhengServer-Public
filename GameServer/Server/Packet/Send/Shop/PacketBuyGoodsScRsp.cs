using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Shop;

public class PacketBuyGoodsScRsp : BasePacket
{
    public PacketBuyGoodsScRsp(BuyGoodsCsReq req, List<ItemData> items) : base(CmdIds.BuyGoodsScRsp)
    {
        var proto = new BuyGoodsScRsp
        {
            ShopId = req.ShopId,
            GoodsBuyTimes = req.GoodsNum,
            GoodsId = req.GoodsId,
            ReturnItemList = new ItemList
            {
                ItemList_ = { items.Select(item => item.ToProto()) }
            }
        };

        SetData(proto);
    }
}