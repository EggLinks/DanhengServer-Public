using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.ChessRogueSelectCellCsReq)]
public class HandlerChessRogueSelectCellCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ChessRogueSelectCellCsReq.Parser.ParseFrom(data);

        if (connection.Player!.ChessRogueManager?.RogueInstance == null) return;
        await connection.Player!.ChessRogueManager!.RogueInstance.SelectCell((int)req.CellId);
    }
}