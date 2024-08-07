using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;

public class PacketGetFriendApplyListInfoCsReq : BasePacket
{
    public PacketGetFriendApplyListInfoCsReq(Connection connection) : base(CmdIds.GetFriendApplyListInfoScRsp)
    {
        SetData(connection.Player!.FriendManager!.ToApplyListProto());
    }
}