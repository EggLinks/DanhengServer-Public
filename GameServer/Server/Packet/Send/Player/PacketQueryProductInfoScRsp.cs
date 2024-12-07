using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketQueryProductInfoScRsp : BasePacket
{
    public PacketQueryProductInfoScRsp() : base(CmdIds.QueryProductInfoScRsp)
    {
        var proto = new QueryProductInfoScRsp
        {
            //PEKJLBINDGG = (ulong)Extensions.GetUnixSec() + 8640000, // 100 day
        };

        SetData(proto);
    }
}