using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail
{
    public class PacketNewMailScNotify : BasePacket
    {
        public PacketNewMailScNotify(int id) : base(CmdIds.NewMailScNotify)
        {
            var proto = new NewMailScNotify()
            {
                MailIdList = { (uint)id }
            };

            SetData(proto);
        }
    }
}
