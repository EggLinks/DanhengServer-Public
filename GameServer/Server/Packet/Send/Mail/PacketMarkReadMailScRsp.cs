using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;

public class PacketMarkReadMailScRsp : BasePacket
{
    public PacketMarkReadMailScRsp(uint mailId) : base(CmdIds.MarkReadMailScRsp)
    {
        var proto = new MarkReadMailScRsp
        {
            Id = mailId
        };

        SetData(proto);
    }

    public PacketMarkReadMailScRsp(Retcode retcode) : base(CmdIds.MarkReadMailScRsp)
    {
        var proto = new MarkReadMailScRsp
        {
            Retcode = (uint)retcode
        };

        SetData(proto);
    }
}