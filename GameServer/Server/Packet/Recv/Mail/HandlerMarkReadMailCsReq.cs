using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mail;

[Opcode(CmdIds.MarkReadMailCsReq)]
public class HandlerMarkReadMailCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = MarkReadMailCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        var mail = player.MailManager!.GetMail((int)req.Id);

        if (mail != null)
        {
            mail.IsRead = true;
            await connection.SendPacket(new PacketMarkReadMailScRsp(req.Id));
        }
        else
        {
            await connection.SendPacket(new PacketMarkReadMailScRsp(Retcode.RetMailMailIdInvalid));
        }
    }
}