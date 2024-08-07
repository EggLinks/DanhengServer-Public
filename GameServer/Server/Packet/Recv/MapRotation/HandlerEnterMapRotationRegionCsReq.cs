using EggLink.DanhengServer.GameServer.Server.Packet.Send.MapRotation;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.MapRotation;

[Opcode(CmdIds.EnterMapRotationRegionCsReq)]
public class HandlerEnterMapRotationRegionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = EnterMapRotationRegionCsReq.Parser.ParseFrom(data);
        await connection.SendPacket(new PacketEnterMapRotationRegionScRsp(req.Motion));
    }
}