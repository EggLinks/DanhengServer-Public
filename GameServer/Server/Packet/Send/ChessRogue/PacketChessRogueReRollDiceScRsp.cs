using EggLink.DanhengServer.GameServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueReRollDiceScRsp : BasePacket
{
    public PacketChessRogueReRollDiceScRsp(ChessRogueDiceInstance dice) : base(CmdIds.ChessRogueReRollDiceScRsp)
    {
        var proto = new ChessRogueReRollDiceScRsp
        {
            RogueDiceInfo = dice.ToProto()
        };

        SetData(proto);
    }
}