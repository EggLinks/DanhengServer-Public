using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.HeartDial;

[Opcode(CmdIds.SubmitEmotionItemCsReq)]
public class HandlerSubmitEmotionItemCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SubmitEmotionItemCsReq.Parser.ParseFrom(data);

        GameData.HeartDialScriptData.TryGetValue((int)req.ScriptId, out var scriptData);
        if (scriptData != null)
        {
            var info = connection.Player!.HeartDialData!.ChangeScriptEmotion((int)req.ScriptId,
                scriptData.DefaultEmoType, HeartDialStepTypeEnum.UnLock);
            await connection.Player!.SendPacket(
                new PacketHeartDialScriptChangeScNotify(HeartDialUnlockStatus.UnlockAll, info));
            await connection.Player!.MissionManager!.HandleFinishType(MissionFinishTypeEnum.HeartDialScriptListStep);
        }

        await connection.SendPacket(new PacketSubmitEmotionItemScRsp(req.ScriptId));
    }
}