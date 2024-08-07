using EggLink.DanhengServer.GameServer.Server.Packet.Send.TalkEvent;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.TalkEvent;

[Opcode(CmdIds.GetFirstTalkByPerformanceNpcCsReq)]
public class HandlerGetFirstTalkByPerformanceNpcCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetFirstTalkByPerformanceNpcCsReq.Parser.ParseFrom(data);
        await connection.SendPacket(new PacketGetFirstTalkByPerformanceNpcScRsp(req));
    }
}