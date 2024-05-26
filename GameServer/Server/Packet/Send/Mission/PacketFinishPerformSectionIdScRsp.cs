using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketFinishPerformSectionIdScRsp : BasePacket
    {
        public PacketFinishPerformSectionIdScRsp(uint sectionId) : base(CmdIds.FinishPerformSectionIdScRsp)
        {
            var proto = new FinishPerformSectionIdScRsp
            {
                SectionId = sectionId
            };

            SetData(proto);
        }
    }
}
