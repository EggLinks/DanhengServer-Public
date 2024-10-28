using EggLink.DanhengServer.GameServer.Server.Packet.Send.Phone;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Phone;

[Opcode(CmdIds.SelectChatBubbleCsReq)]
public class HandlerSelectChatBubbleCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SelectChatBubbleCsReq.Parser.ParseFrom(data);

        connection.Player!.Data.ChatBubble = (int)req.BubbleId;

        await connection.SendPacket(new PacketSelectChatBubbleScRsp(req.BubbleId));
    }
}