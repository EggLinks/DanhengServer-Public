using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.HeartDial;

[Opcode(CmdIds.FinishEmotionDialoguePerformanceCsReq)]
public class HandlerFinishEmotionDialoguePerformanceCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = FinishEmotionDialoguePerformanceCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;
        await player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.HeartDialDialoguePerformanceFinish,
            $"HeartDial_{req.DialogueId}");

        await connection.SendPacket(new PacketFinishEmotionDialoguePerformanceScRsp(req.ScriptId, req.DialogueId));
    }
}