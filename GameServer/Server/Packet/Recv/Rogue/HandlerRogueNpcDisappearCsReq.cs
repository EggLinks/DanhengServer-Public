using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.RogueNpcDisappearCsReq)]
    public class HandlerRogueNpcDisappearCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = RogueNpcDisappearCsReq.Parser.ParseFrom(data);

            connection.Player!.RogueManager!.GetRogueInstance()?.HandleNpcDisappear((int)req.EntityId);

            connection.SendPacket(CmdIds.RogueNpcDisappearScRsp);
        }
    }
}
