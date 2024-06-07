using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Friend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Friend
{
    [Opcode(CmdIds.ApplyFriendCsReq)]
    public class HandlerApplyFriendCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ApplyFriendCsReq.Parser.ParseFrom(data);

            connection.Player!.FriendManager!.AddFriend((int)req.Uid);

            connection.SendPacket(new PacketApplyFriendScRsp(req.Uid));
        }
    }
}
