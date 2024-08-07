using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mail;

[Opcode(CmdIds.DelMailCsReq)]
public class HandlerDelMailCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = DelMailCsReq.Parser.ParseFrom(data);

        foreach (var id in req.IdList) connection.Player!.MailManager?.DeleteMail((int)id);

        await connection.SendPacket(new PacketDelMailScRsp([..req.IdList]));
    }
}