using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.ChessRogueEnterCellCsReq)]
public class HandlerChessRogueEnterCellCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ChessRogueEnterCellCsReq.Parser.ParseFrom(data);
        await connection.Player!.ChessRogueManager!.RogueInstance!.EnterCell((int)req.CellId, (int)req.SelectMonsterId);

        await connection.SendPacket(new PacketChessRogueEnterCellScRsp(req.CellId,
            connection.Player!.ChessRogueManager!.RogueInstance!));
    }
}