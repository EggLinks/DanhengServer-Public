using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;

public class PacketNewMailScNotify : BasePacket
{
    public PacketNewMailScNotify(int id) : base(CmdIds.NewMailScNotify)
    {
        var proto = new NewMailScNotify
        {
            MailIdList = { (uint)id }
        };

        SetData(proto);
    }
}