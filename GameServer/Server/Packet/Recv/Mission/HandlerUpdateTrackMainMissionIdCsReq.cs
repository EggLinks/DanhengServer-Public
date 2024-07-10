using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.UpdateTrackMainMissionIdCsReq)]
    public class HandlerUpdateTrackMainMissionIdCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = UpdateTrackMainMissionIdCsReq.Parser.ParseFrom(data);

            var prev = connection.Player!.MissionManager!.Data.TrackingMainMissionId;
            connection.Player!.MissionManager!.Data.TrackingMainMissionId = (int)req.TrackMissionId;

            connection.SendPacket(new PacketUpdateTrackMainMissionIdScRsp(prev, connection.Player!.MissionManager!.Data.TrackingMainMissionId));
        }
    }
}
