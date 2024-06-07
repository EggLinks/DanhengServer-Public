using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueNousEditDiceScRsp : BasePacket
    {
        public PacketChessRogueNousEditDiceScRsp(ChessRogueNousDiceData diceData) : base(CmdIds.ChessRogueNousEditDiceScRsp)
        {
            var proto = new ChessRogueNousEditDiceScRsp()
            {
                DiceInfo = diceData.ToProto(),
            };

            SetData(proto);
        }
    }
}
