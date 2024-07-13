using EggLink.DanhengServer.Game.Rogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueCommonVirtualItemInfoScNotify : BasePacket
    {
        public PacketSyncRogueCommonVirtualItemInfoScNotify(BaseRogueInstance instance) : base(CmdIds.SyncRogueCommonVirtualItemInfoScNotify)
        {
            var proto = new SyncRogueCommonVirtualItemInfoScNotify
            {
                CommonItemInfo = { new RogueCommonVirtualItemInfo()
                {
                    VirtualItemId = 31,
                    VirtualItemNum = (uint)instance.CurMoney,
                } }
            };

            SetData(proto);
        }
    }
}
