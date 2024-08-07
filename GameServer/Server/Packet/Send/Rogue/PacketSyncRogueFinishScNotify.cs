using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketSyncRogueFinishScNotify : BasePacket
{
    public PacketSyncRogueFinishScNotify(RogueFinishInfo info) : base(CmdIds.SyncRogueFinishScNotify)
    {
        var proto = new SyncRogueFinishScNotify
        {
            RogueFinishInfo = info
        };

        SetData(proto);
    }
}