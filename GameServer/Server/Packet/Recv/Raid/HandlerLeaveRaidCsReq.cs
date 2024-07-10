using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Lineup;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Raid
{
    [Opcode(CmdIds.LeaveRaidCsReq)]
    public class HandlerLeaveRaidCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var player = connection.Player!;
            var req = LeaveRaidCsReq.Parser.ParseFrom(data);
            player.RaidManager!.LeaveRaid(req.IsSave);

            connection.SendPacket(CmdIds.LeaveRaidScRsp);
        }
    }
}
