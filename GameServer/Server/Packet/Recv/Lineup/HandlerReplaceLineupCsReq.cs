using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Lineup;

[Opcode(CmdIds.ReplaceLineupCsReq)]
public class HandlerReplaceLineupCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ReplaceLineupCsReq.Parser.ParseFrom(data);
        await connection.Player!.LineupManager!.ReplaceLineup(req);
        await connection.SendPacket(CmdIds.ReplaceLineupScRsp);
    }
}