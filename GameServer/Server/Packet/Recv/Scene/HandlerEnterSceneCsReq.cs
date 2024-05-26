using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.EnterSceneCsReq)]
    public class HandlerEnterSceneCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = EnterSceneCsReq.Parser.ParseFrom(data);
            connection.Player?.EnterScene((int)req.EntryId, (int)req.TeleportId, true);

            connection.SendPacket(CmdIds.EnterSceneScRsp);
        }
    }
}
