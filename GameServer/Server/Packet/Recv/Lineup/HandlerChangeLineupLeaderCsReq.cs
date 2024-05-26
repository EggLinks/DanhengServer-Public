using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;

namespace EggLink.DanhengServer.Server.Packet.Recv.Lineup
{
    [Opcode(CmdIds.ChangeLineupLeaderCsReq)]
    public class HandlerChangeLineupLeaderCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ChangeLineupLeaderCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            if (player.LineupManager!.GetCurLineup() == null)
            {
                connection.SendPacket(new PacketChangeLineupLeaderScRsp());
                return;
            }
            var lineup = player.LineupManager!.GetCurLineup()!;
            if (lineup.BaseAvatars?.Count <= (int)req.Slot) 
            {
                connection.SendPacket(new PacketChangeLineupLeaderScRsp());
                return; 
            }
            var leaderAvatarId = lineup.BaseAvatars![(int)req.Slot].BaseAvatarId;
            lineup.LeaderAvatarId = leaderAvatarId;
            // save
            DatabaseHelper.Instance?.UpdateInstance(player.LineupManager!.LineupData);
            connection.SendPacket(new PacketChangeLineupLeaderScRsp(req.Slot));
        }
    }
}
