using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Shop
{
    [Opcode(CmdIds.ComposeItemCsReq)]
    public class HandlerComposeItemCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ComposeItemCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            var item = player.InventoryManager!.ComposeItem((int)req.ComposeId, (int)req.Count);
            if (item == null)
            {
                connection.SendPacket(new PacketComposeItemScRsp());
                return;
            }
            connection.SendPacket(new PacketComposeItemScRsp(req.ComposeId, req.Count, item));
        }
    }
}
