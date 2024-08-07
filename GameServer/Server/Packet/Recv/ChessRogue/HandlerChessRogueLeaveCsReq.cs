using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.ChessRogueLeaveCsReq)]
public class HandlerChessRogueLeaveCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.Player!.ChessRogueManager!.RogueInstance!.LeaveRogue();
    }
}