using EggLink.DanhengServer.Game.ChessRogue;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueLeaveScRsp : BasePacket
    {
        public PacketChessRogueLeaveScRsp(ChessRogueInstance instance) : base(CmdIds.ChessRogueLeaveScRsp)
        {
            var proto = new ChessRogueLeaveScRsp()
            {
                PlayerInfo = instance.ToPlayerProto(),
                QueryInfo = instance.Player.ChessRogueManager!.ToQueryInfo(),
                RogueAeonInfo = instance.Player.ChessRogueManager!.ToRogueAeonInfo(),
                RogueGetInfo = instance.Player.ChessRogueManager!.ToGetInfo(),
            };

            SetData(proto);
        }
    }
}
