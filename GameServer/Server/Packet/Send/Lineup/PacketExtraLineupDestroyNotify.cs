using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;

public class PacketExtraLineupDestroyNotify : BasePacket
{
    public PacketExtraLineupDestroyNotify(ExtraLineupType type) : base(CmdIds.ExtraLineupDestroyNotify)
    {
        var proto = new ExtraLineupDestroyNotify
        {
            ExtraLineupType = type
        };

        SetData(proto);
    }
}