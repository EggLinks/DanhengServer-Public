using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketSyncRogueExploreWinScNotify : BasePacket
{
    public PacketSyncRogueExploreWinScNotify() : base(CmdIds.SyncRogueExploreWinScNotify)
    {
        var proto = new SyncRogueExploreWinScNotify
        {
            IsExploreWin = true
        };

        SetData(proto);
    }
}