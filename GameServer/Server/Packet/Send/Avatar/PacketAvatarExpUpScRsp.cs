using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;

public class PacketAvatarExpUpScRsp : BasePacket
{
    public PacketAvatarExpUpScRsp(List<ItemData> returnItem) : base(CmdIds.AvatarExpUpScRsp)
    {
        var proto = new AvatarExpUpScRsp();
        proto.ReturnItemList.AddRange(returnItem.Select(item => item.ToPileProto()));

        SetData(proto);
    }
}