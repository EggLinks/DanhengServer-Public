using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Lineup;

[Opcode(CmdIds.ChangeLineupLeaderCsReq)]
public class HandlerChangeLineupLeaderCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ChangeLineupLeaderCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        if (player.LineupManager!.GetCurLineup() == null)
        {
            await connection.SendPacket(new PacketChangeLineupLeaderScRsp());
            return;
        }

        var lineup = player.LineupManager!.GetCurLineup()!;
        if (lineup.BaseAvatars?.Count <= (int)req.Slot)
        {
            await connection.SendPacket(new PacketChangeLineupLeaderScRsp());
            return;
        }

        var leaderAvatarId = lineup.BaseAvatars![(int)req.Slot].BaseAvatarId;
        lineup.LeaderAvatarId = leaderAvatarId;
        await player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.TeamLeaderChange);

        await connection.SendPacket(new PacketChangeLineupLeaderScRsp(req.Slot));
    }
}