using EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Rogue;

[Opcode(CmdIds.LeaveRogueCsReq)]
public class HandlerLeaveRogueCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        if (player.RogueManager?.RogueInstance != null) await player.RogueManager.RogueInstance.LeaveRogue();
        await connection.SendPacket(new PacketLeaveRogueScRsp(player));
    }
}