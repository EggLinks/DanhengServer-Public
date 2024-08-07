using EggLink.DanhengServer.GameServer.Server.Packet.Send.Chat;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Chat;

[Opcode(CmdIds.GetPrivateChatHistoryCsReq)]
public class HandlerGetPrivateChatHistoryCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetPrivateChatHistoryCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(new PacketGetPrivateChatHistoryScRsp(req.ContactId, connection.Player!));
    }
}