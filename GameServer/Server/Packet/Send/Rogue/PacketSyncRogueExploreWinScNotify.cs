using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueExploreWinScNotify : BasePacket
    {
        public PacketSyncRogueExploreWinScNotify() : base(CmdIds.SyncRogueExploreWinScNotify)
        {
            var proto = new SyncRogueExploreWinScNotify()
            {
                IsWin = true,
            };

            SetData(proto);
        }
    }
}
