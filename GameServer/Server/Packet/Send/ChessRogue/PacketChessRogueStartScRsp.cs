using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueStartScRsp : BasePacket
    {
        public PacketChessRogueStartScRsp(PlayerInstance player) : base(CmdIds.ChessRogueStartScRsp)
        {
            var proto = new ChessRogueStartScRsp()
            {
                PlayerInfo = player.ChessRogueManager!.RogueInstance!.ToPlayerProto(),
                Info = player.ChessRogueManager!.RogueInstance!.ToProto(),
                RogueCurrentInfo = player.ChessRogueManager!.RogueInstance!.ToRogueGameInfo()
            };

            SetData(proto);
        }
    }
}
