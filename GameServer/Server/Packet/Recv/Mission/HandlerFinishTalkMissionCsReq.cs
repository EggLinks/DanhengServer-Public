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
            
            player.MissionManager!.HandleTalkStr(req.TalkStr);

            if (req.CustomValueList != null && req.CustomValueList.Count > 0)
            {
                player.MissionManager!.HandleCustomValue([.. req.CustomValueList], (int)req.SubMissionId);
            }

            connection.SendPacket(new PacketFinishTalkMissionScRsp(req.TalkStr));
        }
    }
}
