using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.PlayerLogoutCsReq)]
    public class HandlerPlayerLogoutCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(CmdIds.PlayerLogoutScRsp);
            connection.Stop();
        }
    }
}
