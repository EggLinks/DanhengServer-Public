using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Lineup
{
    public class PacketSceneCastSkillMpUpdateScNotify : BasePacket
    {
        public PacketSceneCastSkillMpUpdateScNotify(uint castEntityId, int mpCount) : base(CmdIds.SceneCastSkillMpUpdateScNotify)
        {
            var proto = new SceneCastSkillMpUpdateScNotify()
            {
                CastEntityId = castEntityId,
                Mp = (uint)mpCount,
            };

            SetData(proto);
        }
    }
}
