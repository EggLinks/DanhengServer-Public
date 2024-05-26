using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketDeactivateFarmElementScRsp : BasePacket
    {
        public PacketDeactivateFarmElementScRsp(uint id) : base(CmdIds.DeactivateFarmElementScRsp)
        {
            var proto = new DeactivateFarmElementScRsp()
            {
                EntityId = id
            };

            SetData(proto);
        } 
    }
}
