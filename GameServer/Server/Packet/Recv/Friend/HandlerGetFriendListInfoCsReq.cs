using EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Friend;

[Opcode(CmdIds.GetFriendListInfoCsReq)]
public class HandlerGetFriendListInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetFriendListInfoScRsp(connection));
    }
}