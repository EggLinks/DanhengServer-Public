using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketSubMissionRewardScNotify : BasePacket
{
    public PacketSubMissionRewardScNotify(int subMissionId, List<ItemData> item) : base(
        CmdIds.SubMissionRewardScNotify)
    {
        var proto = new SubMissionRewardScNotify
        {
            SubMissionId = (uint)subMissionId,
            Reward = new ItemList()
        };

        foreach (var i in item) proto.Reward.ItemList_.Add(i.ToProto());

        SetData(proto);
    }
}