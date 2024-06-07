using EggLink.DanhengServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueConfirmRollScRsp : BasePacket
    {
        public PacketChessRogueConfirmRollScRsp(ChessRogueDiceInstance dice) : base(CmdIds.ChessRogueConfirmRollScRsp)
        {
            var proto = new ChessRogueConfirmRollScRsp()
            {
                RogueDiceInfo = dice.ToProto(),
            };

            SetData(proto);
        }
    }
}
