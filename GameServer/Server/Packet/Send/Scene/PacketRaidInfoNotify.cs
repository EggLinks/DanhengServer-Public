using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketRaidInfoNotify : BasePacket
    {
        public PacketRaidInfoNotify(uint raidId, RaidStatus status = RaidStatus.Doing) : base(CmdIds.RaidInfoNotify)
        {
            var proto = new RaidInfoNotify()
            {
                RaidId = raidId,
                Status = status
            };

            SetData(proto);
        }
    }
}
