using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
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
            var overMapTp = connection.Player?.EnterScene((int)req.EntryId, (int)req.TeleportId, true, storyLineId:(int)req.GameStoryLineId, mapTp:req.MapTp);

            connection.SendPacket(new PacketEnterSceneScRsp(overMapTp == true, req.MapTp, (int)req.GameStoryLineId));
        }
    }
}
