using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Raid;

[Opcode(CmdIds.StartRaidCsReq)]
public class HandlerStartRaidCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = StartRaidCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        await player.RaidManager!.EnterRaid((int)req.RaidId, (int)req.WorldLevel,
            req.AvatarList.Select(x => (int)x).ToList(),
            req.IsSaveData == 1);

        await connection.SendPacket(CmdIds.StartRaidScRsp);
    }
}