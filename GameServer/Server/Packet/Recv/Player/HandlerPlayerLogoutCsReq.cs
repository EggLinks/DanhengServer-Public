using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.PlayerLogoutCsReq)]
public class HandlerPlayerLogoutCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(CmdIds.PlayerLogoutScRsp);
        connection.Stop();
    }
}