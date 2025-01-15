using EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.RelicRecommendCsReq)]
public class HandlerRelicRecommendCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RelicRecommendCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(new PacketRelicRecommendScRsp(req.AvatarId));
    }
}