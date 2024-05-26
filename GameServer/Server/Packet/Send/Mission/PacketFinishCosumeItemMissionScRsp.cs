using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketFinishCosumeItemMissionScRsp : BasePacket
    {
        public PacketFinishCosumeItemMissionScRsp(uint subMissionId) : base(CmdIds.FinishCosumeItemMissionScRsp)
        {
            var proto = new FinishCosumeItemMissionScRsp()
            {
                SubMissionId = subMissionId
            };

            SetData(proto);
        }

        public PacketFinishCosumeItemMissionScRsp() : base(CmdIds.FinishCosumeItemMissionScRsp)
        {
            var proto = new FinishCosumeItemMissionScRsp()
            {
                Retcode = 1
            };
            SetData(proto);
        }
    }
}
