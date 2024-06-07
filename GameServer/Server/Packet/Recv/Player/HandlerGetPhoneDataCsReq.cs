using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.GetPhoneDataCsReq)]
    public class HandlerGetPhoneDataCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetPhoneDataScRsp(connection.Player!));
        }
    }
}
