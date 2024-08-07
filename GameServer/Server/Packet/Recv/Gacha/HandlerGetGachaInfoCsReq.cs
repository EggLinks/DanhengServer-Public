using EggLink.DanhengServer.GameServer.Server.Packet.Send.Gacha;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Gacha;

[Opcode(CmdIds.GetGachaInfoCsReq)]
public class HandlerGetGachaInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetGachaInfoScRsp(connection.Player!));
    }
}