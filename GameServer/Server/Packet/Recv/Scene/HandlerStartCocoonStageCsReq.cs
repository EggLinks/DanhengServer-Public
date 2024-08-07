using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.StartCocoonStageCsReq)]
public class HandlerStartCocoonStageCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = StartCocoonStageCsReq.Parser.ParseFrom(data);
        await connection.Player!.BattleManager!.StartCocoonStage((int)req.CocoonId, (int)req.Wave, (int)req.WorldLevel);
    }
}