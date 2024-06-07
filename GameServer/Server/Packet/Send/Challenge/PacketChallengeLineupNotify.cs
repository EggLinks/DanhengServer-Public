using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Challenge
{
    public class PacketChallengeLineupNotify : BasePacket
    {
        public PacketChallengeLineupNotify(ExtraLineupType type) : base(CmdIds.ChallengeLineupNotify)
        {
            var proto = new ChallengeLineupNotify()
            {
                ExtraLineupType = type
            };

            SetData(proto);
        }
    }
}
