using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.ChessRogueRollDiceCsReq)]
public class HandlerChessRogueRollDiceCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.Player!.ChessRogueManager!.RogueInstance!.RollDice();
    }
}