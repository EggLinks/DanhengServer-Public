using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission
{
    public class PacketChangeStoryLineFinishScNotify : BasePacket
    {
        public PacketChangeStoryLineFinishScNotify(int curId, ChangeStoryLineAction reason) : base(CmdIds.ChangeStoryLineFinishScNotify)
        {
            var proto = new ChangeStoryLineFinishScNotify
            {
                ActionType = reason,
                CurStoryLineId = (uint)curId,
            };

            SetData(proto);
        }
    }
}
