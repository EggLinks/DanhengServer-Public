using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.StartRogueCsReq)]
    public class HandlerStartRogueCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = StartRogueCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            var disableAeonIdList = req.DisableAeonIdList.Select(x => (int)x).ToList();
            var avatarIds = req.BaseAvatarIdList.Select(x => (int)x).ToList();

            player.RogueManager!.StartRogue((int) req.AreaId, (int)req.AeonId, disableAeonIdList, avatarIds);
        }
    }
}
