using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.FinishRogueCommonDialogueCsReq)]
    public class HandlerFinishRogueCommonDialogueCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = FinishRogueCommonDialogueCsReq.Parser.ParseFrom(data);

            var rogue = connection.Player!.RogueManager?.GetRogueInstance();
            if (rogue == null) return;

            rogue.HandleFinishDialogueGroup((int)req.EventUniqueId);

            connection.SendPacket(CmdIds.FinishRogueCommonDialogueScRsp);
        }
    }
}
