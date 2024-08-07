using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Friend;

[Opcode(CmdIds.HandleFriendCsReq)]
public class HandlerHandleFriendCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = HandleFriendCsReq.Parser.ParseFrom(data);

        PlayerData? playerData = null;
        if (req.IsAccept)
            playerData = await connection.Player!.FriendManager!.ConfirmAddFriend((int)req.Uid);
        else
            connection.Player!.FriendManager!.RefuseAddFriend((int)req.Uid);

        if (playerData != null)
            await connection.SendPacket(new PacketHandleFriendScRsp(req.Uid, req.IsAccept, playerData));
        else
            await connection.SendPacket(new PacketHandleFriendScRsp(req.Uid, req.IsAccept));
    }
}