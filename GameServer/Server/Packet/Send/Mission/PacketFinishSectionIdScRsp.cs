using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketFinishSectionIdScRsp : BasePacket
    {
        public PacketFinishSectionIdScRsp(uint sectionId) : base(CmdIds.FinishSectionIdScRsp)
        {
            var proto = new FinishSectionIdScRsp
            {
                SectionId = sectionId
            };

            SetData(proto);
        }
    }
}
