using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.ChessRogueNousEditDiceCsReq)]
public class HandlerChessRogueNousEditDiceCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = ChessRogueNousEditDiceCsReq.Parser.ParseFrom(data);

        var diceData = player.ChessRogueManager!.SetDice(req.QueryDiceInfo);

        await connection.SendPacket(new PacketChessRogueNousEditDiceScRsp(diceData));
    }
}