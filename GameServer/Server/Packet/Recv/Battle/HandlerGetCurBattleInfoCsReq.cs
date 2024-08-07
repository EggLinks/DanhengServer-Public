using EggLink.DanhengServer.GameServer.Server.Packet.Send.Battle;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Battle;

[Opcode(CmdIds.GetCurBattleInfoCsReq)]
public class HandlerGetCurBattleInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetCurBattleInfoScRsp());
    }
}