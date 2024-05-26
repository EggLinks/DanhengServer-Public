using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Lineup
{
    public class PacketSceneCastSkillCostMpScRsp : BasePacket
    {
        public PacketSceneCastSkillCostMpScRsp(int entityId) : base(CmdIds.SceneCastSkillCostMpScRsp)
        {
            var proto = new SceneCastSkillCostMpScRsp
            {
                CastEntityId = (uint)entityId,
            };

            SetData(proto);
        }
    }
}
