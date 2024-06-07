using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.GetMainMissionCustomValueCsReq)]
    public class HandlerGetMainMissionCustomValueCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetMainMissionCustomValueCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            connection.SendPacket(new PacketGetMainMissionCustomValueScRsp(req, player));
        }
    }
}
