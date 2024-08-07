using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.InteractPropCsReq)]
public class HandlerInteractPropCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = InteractPropCsReq.Parser.ParseFrom(data);
        var prop = await connection.Player!.InteractProp((int)req.PropEntityId, (int)req.InteractId);
        await connection.SendPacket(new PacketInteractPropScRsp(prop));
    }
}