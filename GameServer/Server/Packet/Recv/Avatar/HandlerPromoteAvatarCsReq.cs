using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.PromoteAvatarCsReq)]
public class HandlerPromoteAvatarCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = PromoteAvatarCsReq.Parser.ParseFrom(data);

        await connection.Player!.InventoryManager!.PromoteAvatar((int)req.BaseAvatarId);

        await connection.SendPacket(CmdIds.PromoteAvatarScRsp);
    }
}