using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;
using Microsoft.EntityFrameworkCore;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.FinishTalkMissionCsReq)]
    public class HandlerFinishTalkMissionCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = FinishTalkMissionCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            try
            {
                var missionId = int.Parse(req.TalkStr.Split('_')[1]);  // may send 0 instead of missionId (in talking)
                if (player.MissionManager!.GetSubMissionStatus(missionId) != Enums.MissionPhaseEnum.Doing)
                {
                    player.MissionManager!.AcceptSubMission(missionId);
                }
                player.MissionManager!.FinishSubMission(missionId);
            } catch
            {
            }

            if (req.CustomValueList != null && req.CustomValueList.Count > 0)
            {
                player.MissionManager!.HandleCustomValue((int)req.CustomValueList[0].Index, (int)req.SubMissionId);
            }

            connection.SendPacket(new PacketFinishTalkMissionScRsp(req.TalkStr));
        }
    }
}
