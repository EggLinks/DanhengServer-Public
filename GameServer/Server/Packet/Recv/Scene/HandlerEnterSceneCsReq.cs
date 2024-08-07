using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.EnterSceneCsReq)]
public class HandlerEnterSceneCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = EnterSceneCsReq.Parser.ParseFrom(data);
        var overMapTp = await connection.Player!.EnterScene((int)req.EntryId, (int)req.TeleportId, true,
            (int)req.GameStoryLineId, req.IsCloseMap);

        await connection.SendPacket(new PacketEnterSceneScRsp(overMapTp, req.IsCloseMap, (int)req.GameStoryLineId));
    }
}