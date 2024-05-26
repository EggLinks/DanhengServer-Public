using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueUpdateActionPointScNotify : BasePacket
    {
        public PacketChessRogueUpdateActionPointScNotify(int actionPoint) : base(CmdIds.ChessRogueUpdateActionPointScNotify)
        {
            var proto = new ChessRogueUpdateActionPointScNotify()
            {
                ActionPoint = actionPoint,
            };

            SetData(proto);
        }
    }
}
