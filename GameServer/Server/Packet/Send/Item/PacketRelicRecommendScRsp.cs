using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;

public class PacketRelicRecommendScRsp : BasePacket
{
    public PacketRelicRecommendScRsp(uint avatar) : base(CmdIds.RelicRecommendScRsp)
    {
        var proto = new RelicRecommendScRsp
        {
            AvatarId = avatar
        };

        SetData(proto);
    }
}