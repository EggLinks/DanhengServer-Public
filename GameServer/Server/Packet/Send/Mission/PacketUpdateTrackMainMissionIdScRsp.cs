using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketUpdateTrackMainMissionIdScRsp : BasePacket
{
    public PacketUpdateTrackMainMissionIdScRsp(int prev, int cur) : base(CmdIds.UpdateTrackMainMissionIdScRsp)
    {
        var proto = new UpdateTrackMainMissionIdScRsp
        {
            PrevTrackMissionId = (uint)prev,
            TrackMissionId = (uint)cur
        };

        SetData(proto);
    }
}