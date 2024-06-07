using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Lineup
{
    [Opcode(CmdIds.SwitchLineupIndexCsReq)]
    public class HandlerSwitchLineupIndexCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SwitchLineupIndexCsReq.Parser.ParseFrom(data);
            if (connection.Player!.LineupManager!.SetCurLineup((int)req.Index))  // SetCurLineup returns true if the index is valid
            {
                connection.SendPacket(new PacketSwitchLineupIndexScRsp(req.Index));
            }
        }
    }
}
