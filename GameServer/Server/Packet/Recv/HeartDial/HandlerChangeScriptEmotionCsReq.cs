using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.HeartDial;

[Opcode(CmdIds.ChangeScriptEmotionCsReq)]
public class HandlerChangeScriptEmotionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ChangeScriptEmotionCsReq.Parser.ParseFrom(data);

        connection.Player!.HeartDialData!.ChangeScriptEmotion((int)req.ScriptId,
            (HeartDialEmoTypeEnum)req.TargetEmotionType);

        await connection.SendPacket(new PacketChangeScriptEmotionScRsp(req.ScriptId, req.TargetEmotionType));
    }
}