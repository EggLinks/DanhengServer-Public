using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Friend
{
    public class PacketApplyFriendScRsp : BasePacket
    {
        public PacketApplyFriendScRsp(uint uid) : base(CmdIds.ApplyFriendScRsp)
        {
            var proto = new ApplyFriendScRsp
            {
                Uid = uid,
            };

            SetData(proto);
        }
    }
}
