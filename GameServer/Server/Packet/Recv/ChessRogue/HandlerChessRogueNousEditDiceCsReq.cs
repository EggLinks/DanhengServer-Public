using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.ChessRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.ChessRogue
{
    [Opcode(CmdIds.ChessRogueNousEditDiceCsReq)]
    public class HandlerChessRogueNousEditDiceCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var player = connection.Player!;
            var req = ChessRogueNousEditDiceCsReq.Parser.ParseFrom(data);

            var diceData = player.ChessRogueManager!.SetDice(req.DiceInfo);

            connection.SendPacket(new PacketChessRogueNousEditDiceScRsp(diceData));
        }
    }
}
