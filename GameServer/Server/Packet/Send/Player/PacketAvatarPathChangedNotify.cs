using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketAvatarPathChangedNotify : BasePacket
{
    public PacketAvatarPathChangedNotify(uint baseAvatarId, MultiPathAvatarType type) : base(
        CmdIds.AvatarPathChangedNotify)
    {
        var proto = new AvatarPathChangedNotify
        {
            BaseAvatarId = baseAvatarId,
            CurMultiPathAvatarType = type
        };

        SetData(proto);
    }
}