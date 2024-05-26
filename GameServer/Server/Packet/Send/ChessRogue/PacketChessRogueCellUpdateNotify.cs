using EggLink.DanhengServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueCellUpdateNotify : BasePacket
    {
        public PacketChessRogueCellUpdateNotify(ChessRogueCellInstance cell, int boardId) : base(CmdIds.ChessRogueCellUpdateNotify)
        {
            var proto = new ChessRogueCellUpdateNotify()
            {
                BoardId = (uint)boardId,
            };

            proto.CellList.Add(cell.ToProto());

            SetData(proto);
        }
    }
}
