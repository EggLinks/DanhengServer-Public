using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Battle;

[Opcode(CmdIds.QuitBattleCsReq)]
public class HandlerQuitBattleCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        connection.Player!.BattleInstance = null;
        await connection.SendPacket(CmdIds.QuitBattleScRsp);
    }
}