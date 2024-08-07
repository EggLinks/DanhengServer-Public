using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerBoard;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.PlayerBoard;

[Opcode(CmdIds.SetHeadIconCsReq)]
public class HandlerSetHeadIconCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = SetHeadIconCsReq.Parser.ParseFrom(data);
        if (req == null) return;
        player.Data.HeadIcon = (int)req.Id;

        await connection.SendPacket(new PacketSetHeadIconScRsp(player));
    }
}