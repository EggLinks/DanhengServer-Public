using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueFinishScNotify : BasePacket
    {
        public PacketSyncRogueFinishScNotify(RogueFinishInfo info) : base(CmdIds.SyncRogueFinishScNotify)
        {
            var proto = new SyncRogueFinishScNotify
            {
                FinishInfo = info
            };

            SetData(proto);
        }
    }
}
