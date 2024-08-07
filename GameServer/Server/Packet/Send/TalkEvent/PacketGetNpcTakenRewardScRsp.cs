using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.TalkEvent;

public class PacketGetNpcTakenRewardScRsp : BasePacket
{
    public PacketGetNpcTakenRewardScRsp(uint npcId) : base(CmdIds.GetNpcTakenRewardScRsp)
    {
        var proto = new GetNpcTakenRewardScRsp
        {
            NpcId = npcId
        };
        SetData(proto);
    }
}