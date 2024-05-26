using EggLink.DanhengServer.Server.Packet.Send.ChessRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.ChessRogue
{
    [Opcode(CmdIds.GetChessRogueNousStoryInfoCsReq)]
    public class HandlerGetChessRogueNousStoryInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetChessRogueNousStoryInfoScRsp());
        }
    }
}
