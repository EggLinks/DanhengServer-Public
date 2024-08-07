using EggLink.DanhengServer.GameServer.Server.Packet.Send.Archive;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Archive;

[Opcode(CmdIds.GetArchiveDataCsReq)]
public class HandlerGetArchiveDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetArchiveDataScRsp(connection.Player!));
    }
}