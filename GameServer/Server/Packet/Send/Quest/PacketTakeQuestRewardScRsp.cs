using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Quest;

public class PacketTakeQuestRewardScRsp : BasePacket
{
    public PacketTakeQuestRewardScRsp(Retcode retCode, List<ItemData> items, List<int> succQuestIds) : base(
        CmdIds.TakeQuestRewardScRsp)
    {
        var proto = new TakeQuestRewardScRsp
        {
            Retcode = (uint)retCode,
            Reward = new ItemList
            {
                ItemList_ = { items.Select(x => x.ToProto()) }
            },
            SuccQuestIdList = { succQuestIds.Select(x => (uint)x) }
        };

        SetData(proto);
    }
}