using EggLink.DanhengServer.GameServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueConfirmRollScRsp : BasePacket
{
    public PacketChessRogueConfirmRollScRsp(ChessRogueDiceInstance dice) : base(CmdIds.ChessRogueConfirmRollScRsp)
    {
        var proto = new ChessRogueConfirmRollScRsp
        {
            RogueDiceInfo = dice.ToProto()
        };

        SetData(proto);
    }
}