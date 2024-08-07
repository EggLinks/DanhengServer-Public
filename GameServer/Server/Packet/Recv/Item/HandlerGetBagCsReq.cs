using EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.GetBagCsReq)]
public class HandlerGetBagCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetBagScRsp(connection.Player!));
    }
}