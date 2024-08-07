using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketPlayerKickOutScNotify : BasePacket
{
    public PacketPlayerKickOutScNotify() : base(CmdIds.PlayerKickOutScNotify)
    {
        var proto = new PlayerKickOutScNotify
        {
            KickType = KickType.KickSqueezed
        };
        SetData(proto);
    }
}