using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Server.Packet.Send.Gacha;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Friend
{
    [Opcode(CmdIds.SendMsgCsReq)]
    public class HandlerSendMsgCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SendMsgCsReq.Parser.ParseFrom(data);

            connection.SendPacket(CmdIds.SendMsgScRsp);

            if (req.MessageType == MsgType.CustomText)
            {
                connection.Player!.FriendManager!.SendMessage(connection.Player!.Uid, (int)req.TargetList[0], req.MessageText);
            }
            else if (req.MessageType == MsgType.Emoji)
            {
                connection.Player!.FriendManager!.SendMessage(connection.Player!.Uid, (int)req.TargetList[0], null, (int)req.ExtraId);
            }
        }
    }
}
