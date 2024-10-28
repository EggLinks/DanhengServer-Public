using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueTourn;

[Opcode(CmdIds.RogueTournGetArchiveRepositoryCsReq)]
public class HandlerRogueTournGetArchiveRepositoryCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(CmdIds.RogueTournGetArchiveRepositoryScRsp);
    }
}