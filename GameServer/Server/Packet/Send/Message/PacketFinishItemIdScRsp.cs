using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Message;

public class PacketFinishItemIdScRsp : BasePacket
{
    public PacketFinishItemIdScRsp(uint itemId) : base(CmdIds.FinishItemIdScRsp)
    {
        var proto = new FinishItemIdScRsp
        {
            ItemId = itemId
        };
        SetData(proto);
    }
}