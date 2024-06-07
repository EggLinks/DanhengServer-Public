using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Shop
{
    public class PacketBuyGoodsScRsp : BasePacket
    {
        public PacketBuyGoodsScRsp(BuyGoodsCsReq req, List<ItemData> items) : base(CmdIds.BuyGoodsScRsp)
        {
            var proto = new BuyGoodsScRsp()
            {
                ShopId = req.ShopId,
                GoodsBuyTimes = req.GoodsNum,
                GoodsId = req.GoodsId,
                ReturnItemList = new()
                {
                    ItemList_ = { items.Select(item => item.ToProto()) }
                }
            };

            SetData(proto);
        }
    }
}
