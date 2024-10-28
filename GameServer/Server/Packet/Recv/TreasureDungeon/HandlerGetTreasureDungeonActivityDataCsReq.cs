using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.TreasureDungeon;

[Opcode(CmdIds.GetTreasureDungeonActivityDataCsReq)]
public class HandlerGetTreasureDungeonActivityDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(CmdIds.GetTreasureDungeonActivityDataScRsp);
    }
}