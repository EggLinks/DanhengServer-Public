using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketSyncRogueStatusScNotify : BasePacket
{
    public PacketSyncRogueStatusScNotify(RogueStatus status) : base(CmdIds.SyncRogueStatusScNotify)
    {
        var proto = new SyncRogueStatusScNotify
        {
            Status = status
        };

        SetData(proto);
    }
}