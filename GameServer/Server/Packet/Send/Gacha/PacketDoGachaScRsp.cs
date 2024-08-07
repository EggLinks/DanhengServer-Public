using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Gacha;

public class PacketDoGachaScRsp : BasePacket
{
    public PacketDoGachaScRsp(DoGachaScRsp rsp) : base(CmdIds.DoGachaScRsp)
    {
        SetData(rsp);
    }

    public PacketDoGachaScRsp() : base(CmdIds.DoGachaScRsp)
    {
        var rsp = new DoGachaScRsp
        {
            Retcode = 1
        };
        SetData(rsp);
    }
}