using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Lineup;

[Opcode(CmdIds.GetAllLineupDataCsReq)]
public class HandlerGetAllLineupDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetAllLineupDataScRsp(connection.Player!));
    }
}