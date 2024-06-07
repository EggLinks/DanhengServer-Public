using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueUpdateAllowedSelectCellScNotify : BasePacket
    {
        public PacketChessRogueUpdateAllowedSelectCellScNotify(int boardId, List<int> allowed) : base(CmdIds.ChessRogueUpdateAllowedSelectCellScNotify)
        {
            var proto = new ChessRogueUpdateAllowedSelectCellScNotify()
            {
                BoardId = (uint)boardId,
            };

            foreach (var cell in allowed)
            {
                proto.AllowedSelectCellIdList.Add((uint)cell);
            }

            SetData(proto);
        }
    }
}
