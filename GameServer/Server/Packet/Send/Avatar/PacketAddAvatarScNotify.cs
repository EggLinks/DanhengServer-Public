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
        public PacketAddAvatarScNotify(int avatarId, bool isGacha) : base(CmdIds.AddAvatarScNotify)
        {
            var packet = new AddAvatarScNotify()
            {
                BaseAvatarId = (uint)avatarId,
                IsNew = true,
                Src = isGacha ? AddAvatarSrcState.AddAvatarSrcGacha : AddAvatarSrcState.AddAvatarSrcNone
            };

            SetData(packet);
        }
    }
}
