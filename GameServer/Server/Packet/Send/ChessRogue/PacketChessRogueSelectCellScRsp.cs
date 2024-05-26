using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueSelectCellScRsp : BasePacket
    {
        public PacketChessRogueSelectCellScRsp(int cellId) : base(CmdIds.ChessRogueSelectCellScRsp)
        {
            var proto = new ChessRogueSelectCellScRsp()
            {
                CellId = (uint)cellId,
            };

            SetData(proto);
        }
    }
}
