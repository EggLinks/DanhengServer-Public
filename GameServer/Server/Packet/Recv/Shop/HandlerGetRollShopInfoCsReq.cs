using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Shop
{
    [Opcode(CmdIds.GetRollShopInfoCsReq)]
    public class HandlerGetRollShopInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetRollShopInfoCsReq.Parser.ParseFrom(data);

            connection.SendPacket(new PacketGetRollShopInfoScRsp(req.RollShopId));
        }
    }
}