using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Lineup
{
    public class PacketChangeLineupLeaderScRsp : BasePacket
    {
        public PacketChangeLineupLeaderScRsp(uint slot) : base(CmdIds.ChangeLineupLeaderScRsp)
        {
            var proto = new ChangeLineupLeaderScRsp()
            {
                Slot = slot,
            };

            SetData(proto);
        }

        public PacketChangeLineupLeaderScRsp() : base(CmdIds.ChangeLineupLeaderScRsp)
        {
            var proto = new ChangeLineupLeaderScRsp()
            {
                Retcode = 1,
            };

            SetData(proto);
        }
    }
}
