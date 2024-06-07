using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketMissionRewardScNotify : BasePacket
    {
        public PacketMissionRewardScNotify(int mainMissionId, int subMissionId, ItemList item) : base(CmdIds.MissionRewardScNotify)
        {
            var proto = new MissionRewardScNotify
            {
                MainMissionId = (uint)mainMissionId,
                SubMissionId = (uint)subMissionId,
                Reward = item
            };

            SetData(proto);
        }
    }
}
