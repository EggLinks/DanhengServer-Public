using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;

public class PacketExpUpRelicScRsp : BasePacket
{
    public PacketExpUpRelicScRsp(List<ItemData> leftover) : base(CmdIds.ExpUpRelicScRsp)
    {
        var proto = new ExpUpRelicScRsp();

        foreach (var item in leftover) proto.ReturnItemList.Add(item.ToPileProto());

        SetData(proto);
    }
}