using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;

public class PacketDelMailScRsp : BasePacket
{
    public PacketDelMailScRsp(List<uint> ids) : base(CmdIds.DelMailScRsp)
    {
        var proto = new DelMailScRsp
        {
            IdList = { ids }
        };

        SetData(proto);
    }
}