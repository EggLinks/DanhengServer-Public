using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Friend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Friend
{
    [Opcode(CmdIds.HandleFriendCsReq)]
    public class HandlerHandleFriendCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = HandleFriendCsReq.Parser.ParseFrom(data);

            PlayerData? playerData = null;
            if (req.IsAccept)
            {
                playerData =connection.Player!.FriendManager!.ConfirmAddFriend((int)req.Uid);
            }
            else
            {
                connection.Player!.FriendManager!.RefuseAddFriend((int)req.Uid);
            }

            if (playerData != null)
            {
                connection.SendPacket(new PacketHandleFriendScRsp(req.Uid, req.IsAccept, playerData));
            } else
            {
                connection.SendPacket(new PacketHandleFriendScRsp(req.Uid, req.IsAccept));
            }
        }
    }
}
