using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketGetAssistListScRsp : BasePacket
    {
        public PacketGetAssistListScRsp() : base(CmdIds.GetAssistListScRsp)
        {
            var proto = new GetAssistListScRsp();

            SetData(proto);
        }
    }
}
