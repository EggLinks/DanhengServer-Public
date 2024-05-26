using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.ChessRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.EnhanceChessRogueBuffCsReq)]
    public class HandlerEnhanceChessRogueBuffCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = EnhanceChessRogueBuffCsReq.Parser.ParseFrom(data);

            connection.Player!.ChessRogueManager!.RogueInstance!.EnhanceBuff((int)req.MazeBuffId, RogueActionSource.RogueCommonActionResultSourceTypeEnhance);
            connection.SendPacket(new PacketEnhanceChessRogueBuffScRsp(connection.Player!.ChessRogueManager!.RogueInstance!, req.MazeBuffId));
        }
    }
}
