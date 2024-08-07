using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketMissionRewardScNotify : BasePacket
{
    public PacketMissionRewardScNotify(int mainMissionId, int subMissionId, List<ItemData> item) : base(
        CmdIds.MissionRewardScNotify)
    {
        var proto = new MissionRewardScNotify
        {
            MainMissionId = (uint)mainMissionId,
            SubMissionId = (uint)subMissionId,
            Reward = new ItemList()
        };

        foreach (var i in item) proto.Reward.ItemList_.Add(i.ToProto());

        SetData(proto);
    }
}