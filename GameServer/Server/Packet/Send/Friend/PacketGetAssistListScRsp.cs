using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;

public class PacketGetAssistListScRsp : BasePacket
{
    public PacketGetAssistListScRsp() : base(CmdIds.GetAssistListScRsp)
    {
        var proto = new GetAssistListScRsp();

        SetData(proto);
    }
}