using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Shop
{
    [Opcode(CmdIds.GetShopListCsReq)]
    public class HandlerGetShopListCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetShopListCsReq.Parser.ParseFrom(data);

            connection.SendPacket(new PacketGetShopListScRsp(req.ShopType));
        }
    }
}
