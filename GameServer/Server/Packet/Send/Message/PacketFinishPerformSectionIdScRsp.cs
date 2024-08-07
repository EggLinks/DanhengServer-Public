using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Message;

public class PacketFinishPerformSectionIdScRsp : BasePacket
{
    public PacketFinishPerformSectionIdScRsp(uint sectionId) : base(CmdIds.FinishPerformSectionIdScRsp)
    {
        var proto = new FinishPerformSectionIdScRsp
        {
            SectionId = sectionId
        };

        SetData(proto);
    }
}