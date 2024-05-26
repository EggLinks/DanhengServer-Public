using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketGetNpcTakenRewardScRsp : BasePacket
    {
        public PacketGetNpcTakenRewardScRsp(uint npcId) : base(CmdIds.GetNpcTakenRewardScRsp)
        {
            var proto = new GetNpcTakenRewardScRsp()
            {
                NpcId = npcId,
            };
            SetData(proto);
        }
    }
}
