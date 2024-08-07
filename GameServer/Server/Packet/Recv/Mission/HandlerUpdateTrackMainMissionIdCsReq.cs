using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mission;

[Opcode(CmdIds.UpdateTrackMainMissionIdCsReq)]
public class HandlerUpdateTrackMainMissionIdCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = UpdateTrackMainMissionIdCsReq.Parser.ParseFrom(data);

        var prev = connection.Player!.MissionManager!.Data.TrackingMainMissionId;
        connection.Player!.MissionManager!.Data.TrackingMainMissionId = (int)req.TrackMissionId;

        await connection.SendPacket(new PacketUpdateTrackMainMissionIdScRsp(prev,
            connection.Player!.MissionManager!.Data.TrackingMainMissionId));
    }
}