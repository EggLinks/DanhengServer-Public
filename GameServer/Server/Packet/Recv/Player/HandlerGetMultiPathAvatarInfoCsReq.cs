using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.GetMultiPathAvatarInfoCsReq)]
public class HandlerGetMultiPathAvatarInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        // Hacky way to prevent exploding
        connection.Player!.AvatarManager!.GetHero()!.ValidateHero();

        await connection.SendPacket(new PacketGetMultiPathAvatarInfoScRsp(connection.Player!));
    }
}