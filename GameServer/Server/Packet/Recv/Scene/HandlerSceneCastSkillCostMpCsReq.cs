using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.SceneCastSkillCostMpCsReq)]
public class HandlerSceneCastSkillCostMpCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SceneCastSkillCostMpCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        await player.LineupManager!.CostMp(1, req.CastEntityId);
        await connection.SendPacket(new PacketSceneCastSkillCostMpScRsp((int)req.CastEntityId));
    }
}