using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.HeartDial
{
    [Opcode(CmdIds.FinishEmotionDialoguePerformanceCsReq)]
    public class HandlerFinishEmotionDialoguePerformanceCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = FinishEmotionDialoguePerformanceCsReq.Parser.ParseFrom(data);

            var player = connection.Player!;
            player.MissionManager?.HandleFinishType(MissionFinishTypeEnum.HeartDialDialoguePerformanceFinish, $"HeartDial_{req.DialogueId}");

            connection.SendPacket(new PacketFinishEmotionDialoguePerformanceScRsp(req.ScriptId, req.DialogueId));
        }
    }
}
