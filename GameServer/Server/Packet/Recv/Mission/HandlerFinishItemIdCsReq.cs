using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.FinishItemIdCsReq)]
    public class HandlerFinishItemIdCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = FinishItemIdCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            player.MessageManager!.FinishMessageItem((int)req.ItemId);

            connection.SendPacket(new PacketFinishItemIdScRsp(req.ItemId));
        }
    }
}
