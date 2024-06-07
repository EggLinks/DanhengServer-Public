using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Lineup;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.LeaveRaidCsReq)]
    public class HandlerLeaveRaidCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var player = connection.Player!;
            if (player.CurRaidId == 0)
            {
                connection.SendPacket(CmdIds.LeaveRaidScRsp);
                return;
            }

            GameData.RaidConfigData.TryGetValue(player.CurRaidId * 100 + 0, out var config);
            if (config == null)
            {
                connection.SendPacket(CmdIds.LeaveRaidScRsp);
                return;
            }


            if (player.LineupManager!.GetCurLineup()!.IsExtraLineup())
            {
                player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);
                player.SendPacket(new PacketSyncLineupNotify(player.LineupManager!.GetCurLineup()!));
            }

            player.CurRaidId = 0;
            player.EnterScene(player.OldEntryId, 0, true);
            player.MoveTo(player.LastPos!, player.LastRot!);
            player.OldEntryId = 0;
            player.LastPos = null;
            player.LastRot = null;
            connection.SendPacket(CmdIds.LeaveRaidScRsp);
        }
    }
}
