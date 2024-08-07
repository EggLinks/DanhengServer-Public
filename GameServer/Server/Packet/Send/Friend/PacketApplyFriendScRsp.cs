using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;

public class PacketApplyFriendScRsp : BasePacket
{
    public PacketApplyFriendScRsp(uint uid) : base(CmdIds.ApplyFriendScRsp)
    {
        var proto = new ApplyFriendScRsp
        {
            Uid = uid
        };

        SetData(proto);
    }
}