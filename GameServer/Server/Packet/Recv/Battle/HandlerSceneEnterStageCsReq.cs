using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.SceneEnterStageCsReq)]
    public class HandlerSceneEnterStageCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SceneEnterStageCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            player.BattleManager!.StartStage((int)req.EventId);
        }
    }
}
