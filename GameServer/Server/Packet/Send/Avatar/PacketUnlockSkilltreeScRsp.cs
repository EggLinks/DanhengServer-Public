using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketUnlockSkilltreeScRsp : BasePacket
    {
        public PacketUnlockSkilltreeScRsp() : base(CmdIds.UnlockSkilltreeScRsp)
        {
            var proto = new UnlockSkilltreeScRsp
            {
                Retcode = 1,
            };

            SetData(proto);
        }

        public PacketUnlockSkilltreeScRsp(uint avatarId, uint pointId, uint level) : base(CmdIds.UnlockSkilltreeScRsp)
        {
            var proto = new UnlockSkilltreeScRsp
            {
                BaseAvatarId = avatarId,
                PointId = pointId,
                Level = level,
            };

            SetData(proto);
        }
    }
}
