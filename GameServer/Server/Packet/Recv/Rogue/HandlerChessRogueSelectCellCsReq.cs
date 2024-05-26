using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.ChessRogueSelectCellCsReq)]
    public class HandlerChessRogueSelectCellCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ChessRogueSelectCellCsReq.Parser.ParseFrom(data);

            connection.Player!.ChessRogueManager!.RogueInstance?.SelectCell((int)req.CellId);
        }
    }
}
