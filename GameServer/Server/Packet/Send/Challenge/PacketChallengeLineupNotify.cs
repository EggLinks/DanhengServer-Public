using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;

public class PacketChallengeLineupNotify : BasePacket
{
    public PacketChallengeLineupNotify(ExtraLineupType type) : base(CmdIds.ChallengeLineupNotify)
    {
        var proto = new ChallengeLineupNotify
        {
            ExtraLineupType = type
        };

        SetData(proto);
    }
}