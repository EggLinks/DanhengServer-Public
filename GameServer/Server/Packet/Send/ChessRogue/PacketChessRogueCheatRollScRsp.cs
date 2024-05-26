using EggLink.DanhengServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueCheatRollScRsp : BasePacket
    {
        public PacketChessRogueCheatRollScRsp(ChessRogueDiceInstance dice, int surfaceId) : base(CmdIds.ChessRogueCheatRollScRsp)
        {
            var proto = new ChessRogueCheatRollScRsp()
            {
                RogueDiceInfo = dice.ToProto(),
                SurfaceId = (uint)surfaceId,
            };

            SetData(proto);
        }
    }
}
