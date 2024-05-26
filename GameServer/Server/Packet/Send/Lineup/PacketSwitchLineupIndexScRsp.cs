using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Lineup
{
    public class PacketSwitchLineupIndexScRsp : BasePacket
    {
        public PacketSwitchLineupIndexScRsp(uint index) : base(CmdIds.SwitchLineupIndexScRsp)
        {
            var proto = new SwitchLineupIndexScRsp()
            {
                Index = index,
            };

            SetData(proto);
        }
    } 
}
