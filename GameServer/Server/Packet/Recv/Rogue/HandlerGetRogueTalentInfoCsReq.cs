using EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Rogue;

[Opcode(CmdIds.GetRogueTalentInfoCsReq)]
public class HandlerGetRogueTalentInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetRogueTalentInfoScRsp());
    }
}