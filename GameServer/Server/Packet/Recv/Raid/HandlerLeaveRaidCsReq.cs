using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Raid;

[Opcode(CmdIds.LeaveRaidCsReq)]
public class HandlerLeaveRaidCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = LeaveRaidCsReq.Parser.ParseFrom(data);
        await player.RaidManager!.LeaveRaid(req.IsSaveData);

        await connection.SendPacket(CmdIds.LeaveRaidScRsp);
    }
}