using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.SelectChatBubbleCsReq)]
    public class HandlerSelectChatBubbleCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SelectChatBubbleCsReq.Parser.ParseFrom(data);

            connection.Player!.Data.ChatBubble = (int)req.BubbleId;
            DatabaseHelper.Instance!.UpdateInstance(connection.Player!.Data);

            connection.SendPacket(new PacketSelectChatBubbleScRsp(req.BubbleId));
        }
    }
}
