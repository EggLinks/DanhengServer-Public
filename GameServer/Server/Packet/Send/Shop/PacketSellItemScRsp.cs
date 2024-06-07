using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Shop
{
    public class PacketSellItemScRsp : BasePacket
    {
        public PacketSellItemScRsp(List<ItemData> items) : base(CmdIds.SellItemScRsp)
        {
            var proto = new SellItemScRsp()
            {
                ReturnItemList = new()
                {
                    ItemList_ = { items.Select(x => x.ToProto())}
                }
            };

            SetData(proto);
        }
    }
}
