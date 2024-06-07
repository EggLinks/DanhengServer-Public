using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.AcceptMainMissionCsReq)]
    public class HandlerAcceptMainMissionCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = AcceptMainMissionCsReq.Parser.ParseFrom(data);
            var missionId = req.MainMissionId;

            connection.Player!.MissionManager!.AcceptMainMission((int)missionId);

            connection.SendPacket(new PacketAcceptMainMissionScRsp(missionId));
        }
    }
}
