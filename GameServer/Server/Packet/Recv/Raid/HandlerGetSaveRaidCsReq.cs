using EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Raid;

[Opcode(CmdIds.GetSaveRaidCsReq)]
public class HandlerGetSaveRaidCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetSaveRaidCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(
            new PacketGetSaveRaidScRsp(connection.Player!, (int)req.RaidId, (int)req.WorldLevel));
    }
}