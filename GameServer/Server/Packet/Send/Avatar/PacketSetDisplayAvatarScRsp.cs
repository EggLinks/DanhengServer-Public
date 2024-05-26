using EggLink.DanhengServer.Proto;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketSetDisplayAvatarScRsp : BasePacket
    {
        public PacketSetDisplayAvatarScRsp(RepeatedField<DisplayAvatarData> avatars) : base(CmdIds.SetDisplayAvatarScRsp)
        {
            var proto = new SetDisplayAvatarScRsp();
            proto.DisplayAvatarList.AddRange(avatars);

            SetData(proto);
        }
    }
}
