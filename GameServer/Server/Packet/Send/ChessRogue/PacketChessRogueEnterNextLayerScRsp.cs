using EggLink.DanhengServer.Game.ChessRogue;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueEnterNextLayerScRsp : BasePacket
    {
        public PacketChessRogueEnterNextLayerScRsp(ChessRogueInstance rogue) : base(CmdIds.ChessRogueEnterNextLayerScRsp)
        {
            var proto = new ChessRogueEnterNextLayerScRsp()
            {
                PlayerInfo = rogue.ToPlayerProto(),
                RogueCurrentInfo = rogue.ToRogueGameInfo(),
                RogueInfo = rogue.ToProto(),
            };

            SetData(proto);
        }
    }
}
