using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketEnterMapRotationRegionScRsp : BasePacket
    {
        public PacketEnterMapRotationRegionScRsp(MotionInfo motion) : base(CmdIds.EnterMapRotationRegionScRsp)
        {
            var proto = new EnterMapRotationRegionScRsp
            {
                Motion = motion
            };

            SetData(proto);
        }
    }
}
