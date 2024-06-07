using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketFinishItemIdScRsp : BasePacket
    {
        public PacketFinishItemIdScRsp(uint itemId) : base(CmdIds.FinishItemIdScRsp)
        {
            var proto = new FinishItemIdScRsp
            {
                ItemId = itemId
            };
            SetData(proto);
        }
    }
}
