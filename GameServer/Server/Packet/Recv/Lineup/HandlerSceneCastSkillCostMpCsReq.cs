using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Lineup
{
    [Opcode(CmdIds.SceneCastSkillCostMpCsReq)]
    public class HandlerSceneCastSkillCostMpCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SceneCastSkillCostMpCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            player.LineupManager!.CostMp(1);
            connection.SendPacket(new PacketSceneCastSkillCostMpScRsp((int)req.CastEntityId));
        }
    }
}
