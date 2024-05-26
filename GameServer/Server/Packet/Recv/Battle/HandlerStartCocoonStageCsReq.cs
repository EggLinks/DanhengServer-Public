using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.StartCocoonStageCsReq)]
    public class HandlerStartCocoonStageCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = StartCocoonStageCsReq.Parser.ParseFrom(data);
            connection.Player?.BattleManager?.StartCocoonStage((int)req.CocoonId, (int)req.Wave, (int)req.WorldLevel);
        }
    }
}
