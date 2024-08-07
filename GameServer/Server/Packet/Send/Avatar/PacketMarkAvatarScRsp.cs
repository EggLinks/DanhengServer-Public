using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;

public class PacketMarkAvatarScRsp : BasePacket
{
    public PacketMarkAvatarScRsp(AvatarInfo avatar) : base(CmdIds.MarkAvatarScRsp)
    {
        var proto = new MarkAvatarScRsp
        {
            AvatarId = (uint)avatar.AvatarId,
            IsMarked = avatar.IsMarked
        };

        SetData(proto);
    }
}