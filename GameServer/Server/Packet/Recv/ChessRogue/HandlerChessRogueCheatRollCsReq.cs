using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.ChessRogueCheatRollCsReq)]
public class HandlerChessRogueCheatRollCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ChessRogueCheatRollCsReq.Parser.ParseFrom(data);
        if (connection.Player!.ChessRogueManager?.RogueInstance == null) return;
        await connection.Player!.ChessRogueManager!.RogueInstance.CheatDice((int)req.DiceSurfaceId);
    }
}