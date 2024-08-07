using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mail;

[Opcode(CmdIds.GetMailCsReq)]
public class HandlerGetMailCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetMailScRsp(connection.Player!));
    }
}