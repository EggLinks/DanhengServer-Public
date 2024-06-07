using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Friend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Friend
{
    [Opcode(CmdIds.GetPrivateChatHistoryCsReq)]
    public class HandlerGetPrivateChatHistoryCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetPrivateChatHistoryCsReq.Parser.ParseFrom(data);

            connection.SendPacket(new PacketGetPrivateChatHistoryScRsp(req.ContactId, connection.Player!));
        }
    }
}
