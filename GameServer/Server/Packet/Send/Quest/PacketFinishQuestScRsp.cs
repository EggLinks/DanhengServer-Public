using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Quest;

public class PacketFinishQuestScRsp : BasePacket
{
    public PacketFinishQuestScRsp(Retcode retCode) : base(CmdIds.FinishQuestScRsp)
    {
        var proto = new FinishQuestScRsp
        {
            Retcode = (uint)retCode
        };

        SetData(proto);
    }
}