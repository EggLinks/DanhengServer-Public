using EggLink.DanhengServer.GameServer.Server.Packet.Send.ContentPackage;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.PlayerLoginCsReq)]
public class HandlerPlayerLoginCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        connection.State = SessionStateEnum.ACTIVE;
        await connection.Player!.OnLogin();
        await connection.SendPacket(new PacketPlayerLoginScRsp(connection));
        await connection.SendPacket(new PacketContentPackageSyncDataScNotify());
    }
}