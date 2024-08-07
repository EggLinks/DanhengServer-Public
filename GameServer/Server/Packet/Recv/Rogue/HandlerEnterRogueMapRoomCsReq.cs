using EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Rogue;

[Opcode(CmdIds.EnterRogueMapRoomCsReq)]
public class HandlerEnterRogueMapRoomCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = EnterRogueMapRoomCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        if (player.RogueManager?.RogueInstance == null) return;
        await player.RogueManager!.RogueInstance.EnterRoom((int)req.SiteId);

        await connection.SendPacket(new PacketEnterRogueMapRoomScRsp(player));
    }
}