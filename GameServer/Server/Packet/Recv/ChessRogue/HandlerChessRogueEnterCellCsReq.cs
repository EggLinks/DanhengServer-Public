using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.ChessRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.ChessRogue
{
    [Opcode(CmdIds.ChessRogueEnterCellCsReq)]
    public class HandlerChessRogueEnterCellCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ChessRogueEnterCellCsReq.Parser.ParseFrom(data);
            connection.Player!.ChessRogueManager!.RogueInstance?.EnterCell((int)req.CellId, (int)req.SelectMonsterId);

            connection.SendPacket(new PacketChessRogueEnterCellScRsp(req.CellId, connection.Player!.ChessRogueManager!.RogueInstance!));
        }
    }
}
