using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.TalkEvent;

public class PacketSelectInclinationTextScRsp : BasePacket
{
    public PacketSelectInclinationTextScRsp(uint id) : base(CmdIds.SelectInclinationTextScRsp)
    {
        var proto = new SelectInclinationTextScRsp
        {
            TalkSentenceId = id
        };

        SetData(proto);
    }
}