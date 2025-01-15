using EggLink.DanhengServer.GameServer.Server.Packet.Send.MatchThreeModule;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.MatchThreeModule;

[Opcode(CmdIds.MatchThreeGetDataCsReq)]
public class HandlerMatchThreeGetDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketMatchThreeGetDataScRsp(connection.Player!));
    }
}