using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using LineupInfo = EggLink.DanhengServer.Database.Lineup.LineupInfo;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;

public class PacketSyncLineupNotify : BasePacket
{
    public PacketSyncLineupNotify(LineupInfo info, SyncLineupReason reason = SyncLineupReason.SyncReasonNone) : base(
        CmdIds.SyncLineupNotify)
    {
        var proto = new SyncLineupNotify
        {
            Lineup = info.ToProto()
        };

        if (reason != SyncLineupReason.SyncReasonNone) proto.ReasonList.Add(reason);

        SetData(proto);
    }
}