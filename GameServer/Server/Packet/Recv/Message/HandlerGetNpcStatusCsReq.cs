using EggLink.DanhengServer.GameServer.Server.Packet.Send.Message;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Message;

[Opcode(CmdIds.GetNpcStatusCsReq)]
public class HandlerGetNpcStatusCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetNpcStatusScRsp(connection.Player!));
    }
}