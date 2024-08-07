using EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Raid;

[Opcode(CmdIds.GetRaidInfoCsReq)]
public class HandlerGetRaidInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetRaidInfoScRsp(connection.Player!));
    }
}