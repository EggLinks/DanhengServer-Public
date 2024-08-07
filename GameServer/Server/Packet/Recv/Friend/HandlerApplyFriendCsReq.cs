using EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Friend;

[Opcode(CmdIds.ApplyFriendCsReq)]
public class HandlerApplyFriendCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ApplyFriendCsReq.Parser.ParseFrom(data);

        await connection.Player!.FriendManager!.AddFriend((int)req.Uid);

        await connection.SendPacket(new PacketApplyFriendScRsp(req.Uid));
    }
}