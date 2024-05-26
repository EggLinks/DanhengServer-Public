using EggLink.DanhengServer.Game.ChessRogue;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueLayerAccountInfoNotify : BasePacket
    { 
        public PacketChessRogueLayerAccountInfoNotify(ChessRogueInstance rogue) : base(CmdIds.ChessRogueLayerAccountInfoNotify)
        {
            var proto = new ChessRogueLayerAccountInfoNotify()
            {
                FinishInfo = rogue.ToFinishInfo(),
                //LayerId = (uint)rogue.CurLayer,
            };

            SetData(proto);
        }
    }
}
