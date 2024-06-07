using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.PlayerHeartBeatCsReq)]
    public class HandlerPlayerHeartBeatCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = PlayerHeartBeatCsReq.Parser.ParseFrom(data);
            if (req != null)
            {
                connection.SendPacket(new PacketPlayerHeartBeatScRsp((long)req.ClientTimeMs));
            }

            connection.Player?.OnHeartBeat();
        }
    }
}
