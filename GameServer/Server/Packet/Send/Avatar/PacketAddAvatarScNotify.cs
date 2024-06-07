using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketAddAvatarScNotify : BasePacket
    {
        public PacketAddAvatarScNotify(int avatarId) : base(CmdIds.AddAvatarScNotify)
        {
            var packet = new AddAvatarScNotify()
            {
                BaseAvatarId = (uint)avatarId,
                IsNew = true,
                Src = AddAvatarSrcState.AddAvatarSrcGacha,
            };

            SetData(packet);
        }
    }
}
