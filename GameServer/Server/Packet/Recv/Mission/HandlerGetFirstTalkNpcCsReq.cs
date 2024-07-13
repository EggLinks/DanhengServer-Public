using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.GetFirstTalkNpcCsReq)]
    public class HandlerGetFirstTalkNpcCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(CmdIds.GetFirstTalkNpcScRsp);
        }
    }
}
