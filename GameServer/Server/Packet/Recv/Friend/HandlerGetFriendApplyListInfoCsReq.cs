using EggLink.DanhengServer.Server.Packet.Send.Gacha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Friend
{
    [Opcode(CmdIds.GetFriendApplyListInfoCsReq)]
    public class HandlerGetFriendApplyListInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetFriendApplyListInfoCsReq(connection));
        }
    }
}
