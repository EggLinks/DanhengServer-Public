using EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Challenge;

[Opcode(CmdIds.GetChallengeCsReq)]
public class HandlerGetChallengeCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetChallengeScRsp(connection.Player!));
    }
}