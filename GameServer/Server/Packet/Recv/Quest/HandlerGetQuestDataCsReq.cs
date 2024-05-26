using EggLink.DanhengServer.Server.Packet.Send.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Quest
{
    [Opcode(CmdIds.GetQuestDataCsReq)]
    public class HandlerGetQuestDataCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetQuestDataScRsp());
        }
    }
}
