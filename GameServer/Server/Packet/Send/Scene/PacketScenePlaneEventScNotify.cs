using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketScenePlaneEventScNotify : BasePacket
{
    public PacketScenePlaneEventScNotify(ItemData item) : this([item])
    {
    }

    public PacketScenePlaneEventScNotify(List<ItemData> itemDatas) : base(CmdIds.ScenePlaneEventScNotify)
    {
        var itemList = new ItemList();
        foreach (var item in itemDatas) itemList.ItemList_.Add(item.ToProto());

        var data = new ScenePlaneEventScNotify
        {
            GetItemList = itemList
        };

        SetData(data);
    }

    public PacketScenePlaneEventScNotify(ItemList list) : base(CmdIds.ScenePlaneEventScNotify)
    {
        var data = new ScenePlaneEventScNotify
        {
            GetItemList = list
        };

        SetData(data);
    }
}