using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.ChessRogue
{
    [Opcode(CmdIds.ChessRogueRollDiceCsReq)]
    public class HandlerChessRogueRollDiceCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.Player!.ChessRogueManager!.RogueInstance?.RollDice();
        }
    }
}
