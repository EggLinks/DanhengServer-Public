using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Lineup
{
    public class PacketSyncLineupNotify : BasePacket
    {
        public PacketSyncLineupNotify(Database.Lineup.LineupInfo info, SyncLineupReason reason = SyncLineupReason.SyncReasonNone) : base(CmdIds.SyncLineupNotify)
        {
            var proto = new SyncLineupNotify()
            {
                Lineup = info.ToProto(),
                ReasonList = { reason }
            };

            SetData(proto);
        }
    }
}
