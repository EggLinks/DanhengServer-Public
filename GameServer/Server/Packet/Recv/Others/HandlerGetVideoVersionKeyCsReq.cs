using EggLink.DanhengServer.Server.Packet.Send.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Others
{
    [Opcode(CmdIds.GetVideoVersionKeyCsReq)]
    public class HandlerGetVideoVersionKeyCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetVideoVersionKeyScRsp());
        }
    }
}
