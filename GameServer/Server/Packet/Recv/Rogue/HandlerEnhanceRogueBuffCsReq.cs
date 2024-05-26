using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.EnhanceRogueBuffCsReq)]
    public class HandlerEnhanceRogueBuffCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = EnhanceRogueBuffCsReq.Parser.ParseFrom(data);

            var rogue = connection.Player!.RogueManager?.GetRogueInstance();
            if (rogue == null) return;

            rogue.EnhanceBuff((int)req.MazeBuffId, RogueActionSource.RogueCommonActionResultSourceTypeEnhance);

            connection.SendPacket(new PacketEnhanceRogueBuffScRsp(req.MazeBuffId));
        }
    }
}
