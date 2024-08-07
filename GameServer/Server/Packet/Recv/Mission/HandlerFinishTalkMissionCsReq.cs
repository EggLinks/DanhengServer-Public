using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mission;

[Opcode(CmdIds.FinishTalkMissionCsReq)]
public class HandlerFinishTalkMissionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = FinishTalkMissionCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        await player.MissionManager!.HandleTalkStr(req.TalkStr);

        if (req.CustomValueList != null && req.CustomValueList.Count > 0)
            await player.MissionManager!.HandleCustomValue([.. req.CustomValueList], (int)req.SubMissionId);

        await connection.SendPacket(new PacketFinishTalkMissionScRsp(req.TalkStr));
    }
}