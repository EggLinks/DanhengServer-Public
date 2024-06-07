using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.EnterRogueMapRoomCsReq)]
    public class HandlerEnterRogueMapRoomCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = EnterRogueMapRoomCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            player.RogueManager!.RogueInstance?.EnterRoom((int)req.SiteId);

            connection.SendPacket(new PacketEnterRogueMapRoomScRsp(player));
        }
    }
}
