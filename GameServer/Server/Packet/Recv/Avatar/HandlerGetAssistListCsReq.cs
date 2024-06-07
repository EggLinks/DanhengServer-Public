using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.GetAssistListCsReq)]
    public class HandlerGetAssistListCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetAssistListScRsp());
        }
    }
}
