using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.FinishPerformSectionIdCsReq)]
    public class HandlerFinishPerformSectionIdCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = FinishPerformSectionIdCsReq.Parser.ParseFrom(data);

            connection.Player!.MessageManager!.FinishSection((int)req.SectionId);

            connection.SendPacket(new PacketFinishPerformSectionIdScRsp(req.SectionId));
        }
    }
}
