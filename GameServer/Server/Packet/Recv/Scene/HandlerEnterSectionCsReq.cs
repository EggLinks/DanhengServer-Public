using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.EnterSectionCsReq)]
public class HandlerEnterSectionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = EnterSectionCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        player.EnterSection((int)req.SectionId);
        await connection.SendPacket(CmdIds.EnterSectionScRsp);
    }
}