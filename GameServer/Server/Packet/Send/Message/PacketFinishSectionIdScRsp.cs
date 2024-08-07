using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Message;

public class PacketFinishSectionIdScRsp : BasePacket
{
    public PacketFinishSectionIdScRsp(uint sectionId) : base(CmdIds.FinishSectionIdScRsp)
    {
        var proto = new FinishSectionIdScRsp
        {
            SectionId = sectionId
        };

        SetData(proto);
    }
}