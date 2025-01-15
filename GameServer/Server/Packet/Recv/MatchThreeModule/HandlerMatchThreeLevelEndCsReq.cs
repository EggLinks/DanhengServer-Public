using EggLink.DanhengServer.GameServer.Server.Packet.Send.MatchThreeModule;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.MatchThreeModule;

[Opcode(CmdIds.MatchThreeLevelEndCsReq)]
public class HandlerMatchThreeLevelEndCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = MatchThreeLevelEndCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(new PacketMatchThreeLevelEndScRsp(req.LevelId, req.ModeId));
    }
}