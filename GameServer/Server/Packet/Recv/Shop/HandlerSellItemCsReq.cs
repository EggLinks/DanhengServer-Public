using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Shop
{
    [Opcode(CmdIds.SellItemCsReq)]
    public class HandlerSellItemCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SellItemCsReq.Parser.ParseFrom(data);
            var items = connection.Player!.InventoryManager!.SellItem(req.CostData);
            connection.SendPacket(new PacketSellItemScRsp(items));
        }
    }
}
