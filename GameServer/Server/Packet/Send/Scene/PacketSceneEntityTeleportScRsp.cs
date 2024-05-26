using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketSceneEntityTeleportScRsp : BasePacket
    {
        public PacketSceneEntityTeleportScRsp(EntityMotion motion) : base(CmdIds.SceneEntityTeleportScRsp)
        {
            var proto = new SceneEntityTeleportScRsp()
            {
                EntityMotion = motion,
            };

            SetData(proto);
        }
    }
}
