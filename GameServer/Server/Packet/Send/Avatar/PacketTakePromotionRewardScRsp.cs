using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;

public class PacketTakePromotionRewardScRsp : BasePacket
{
    public PacketTakePromotionRewardScRsp() : base(CmdIds.TakePromotionRewardScRsp)
    {
        var itemList = new Proto.Item
        {
            ItemId = 101,
            Num = 1
        };

        var rsp = new TakePromotionRewardScRsp
        {
            RewardList = new ItemList
            {
                ItemList_ = { itemList }
            }
        };

        SetData(rsp);
    }
}