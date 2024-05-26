using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Lineup
{
    [Opcode(CmdIds.ReplaceLineupCsReq)]
    public class HandlerReplaceLineupCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ReplaceLineupCsReq.Parser.ParseFrom(data);
            connection.Player!.LineupManager?.ReplaceLineup(req);
            connection.SendPacket(CmdIds.ReplaceLineupScRsp);
        }
    }
}
