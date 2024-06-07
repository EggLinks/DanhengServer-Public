using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.GetRogueInfoCsReq)]
    public class HandlerGetRogueInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetRogueInfoScRsp(connection.Player!));
        }
    }
}
