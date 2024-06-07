using EggLink.DanhengServer.Server.Packet.Send.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.GetNpcStatusCsReq)]
    public class HandlerGetNpcStatusCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetNpcStatusScRsp(connection.Player!));
        }
    }
}
