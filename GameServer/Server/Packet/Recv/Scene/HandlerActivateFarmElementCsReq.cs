using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.ActivateFarmElementCsReq)]
public class HandlerActivateFarmElementCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ActivateFarmElementCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(new PacketActivateFarmElementScRsp(req.EntityId, connection.Player!));
    }
}