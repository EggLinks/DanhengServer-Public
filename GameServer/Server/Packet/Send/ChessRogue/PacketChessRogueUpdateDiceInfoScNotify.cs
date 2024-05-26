using EggLink.DanhengServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueUpdateDiceInfoScNotify : BasePacket
    {
        public PacketChessRogueUpdateDiceInfoScNotify(ChessRogueDiceInstance dice) : base(CmdIds.ChessRogueUpdateDiceInfoScNotify)
        {
            var proto = new ChessRogueUpdateDiceInfoScNotify()
            {
                RogueDiceInfo = dice.ToProto(),
            };

            SetData(proto);
        }
    }
}
