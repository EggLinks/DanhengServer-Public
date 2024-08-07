using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerBoard;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.PlayerBoard;

[Opcode(CmdIds.GetPlayerBoardDataCsReq)]
public class HandlerGetPlayerBoardDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetPlayerBoardDataScRsp(connection.Player!));
    }
}