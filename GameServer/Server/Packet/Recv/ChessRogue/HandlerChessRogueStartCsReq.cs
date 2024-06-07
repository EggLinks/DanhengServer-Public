using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.ChessRogue
{
    [Opcode(CmdIds.ChessRogueStartCsReq)]
    public class HandlerChessRogueStartCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var player = connection.Player!;
            var req = ChessRogueStartCsReq.Parser.ParseFrom(data);

            var difficultyIdList = new List<int>();
            var disableAeonIdList = new List<int>();

            if (req.DifficultyIdList != null)
            {
                foreach (var difficultyId in req.DifficultyIdList)
                {
                    difficultyIdList.Add((int)difficultyId);
                }
            }

            if (req.DisableAeonIdList != null)
            {
                foreach (var disableAeonId in req.DisableAeonIdList)
                {
                    disableAeonIdList.Add((int)disableAeonId);
                }
            }

            player.ChessRogueManager!.StartRogue((int)req.AeonId, [.. req.BaseAvatarIdList], (int)req.Id, (int)req.BranchId, difficultyIdList, disableAeonIdList);
        }
    }
}
