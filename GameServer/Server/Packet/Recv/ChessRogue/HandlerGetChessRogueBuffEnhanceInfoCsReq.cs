using EggLink.DanhengServer.Server.Packet.Send.ChessRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.ChessRogue
{
    [Opcode(CmdIds.GetChessRogueBuffEnhanceInfoCsReq)]
    public class HandlerGetChessRogueBuffEnhanceInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetChessRogueBuffEnhanceInfoScRsp(connection.Player!));
        }
    }
}
