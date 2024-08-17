using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketMonthCardRewardNotify : BasePacket
{
    public PacketMonthCardRewardNotify(List<ItemData> items) : base(CmdIds.MonthCardRewardNotify)
    {
        var proto = new MonthCardRewardNotify
        {
            Reward = new ItemList
            {
                ItemList_ = { items.Select(x => x.ToProto()) }
            }
        };

        SetData(proto);
    }
}