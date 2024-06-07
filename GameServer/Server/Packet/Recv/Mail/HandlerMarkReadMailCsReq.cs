using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mail
{
    [Opcode(CmdIds.MarkReadMailCsReq)]
    public class HandlerMarkReadMailCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = MarkReadMailCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;

            var mail = player.MailManager!.GetMail((int)req.Id);

            if (mail != null)
            {
                mail.IsRead = true;
                connection.SendPacket(new PacketMarkReadMailScRsp(req.Id));
            } else
            {
                connection.SendPacket(new PacketMarkReadMailScRsp(Retcode.RetMailMailIdInvalid));
            }
        }
    }
}
