using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.GetChessRogueBuffEnhanceInfoCsReq)]
public class HandlerGetChessRogueBuffEnhanceInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetChessRogueBuffEnhanceInfoScRsp(connection.Player!));
    }
}