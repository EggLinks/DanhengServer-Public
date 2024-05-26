using EggLink.DanhengServer.Game.Rogue;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueVirtualItemScNotify : BasePacket
    {
        public PacketSyncRogueVirtualItemScNotify(BaseRogueInstance instance): base(CmdIds.SyncRogueVirtualItemInfoScNotify)
        {
            var proto = new SyncRogueVirtualItemInfoScNotify
            {
                RogueVirtualItemInfo = new()
                {
                    //RogueMoney = (uint)instance.CurMoney,
                }
            };

            SetData(proto);
        }
    }
}
