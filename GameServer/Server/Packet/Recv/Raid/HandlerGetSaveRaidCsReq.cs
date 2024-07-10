using EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Raid
{
    [Opcode(CmdIds.GetSaveRaidCsReq)]
    public class HandlerGetSaveRaidCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetSaveRaidCsReq.Parser.ParseFrom(data);

            connection.SendPacket(new PacketGetSaveRaidScRsp(connection.Player!, (int)req.RaidId, (int)req.WorldLevel));
        }
    }
}
