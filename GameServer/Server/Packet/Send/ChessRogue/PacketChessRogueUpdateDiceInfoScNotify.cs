using EggLink.DanhengServer.GameServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueUpdateDiceInfoScNotify : BasePacket
{
    public PacketChessRogueUpdateDiceInfoScNotify(ChessRogueDiceInstance dice) : base(
        CmdIds.ChessRogueUpdateDiceInfoScNotify)
    {
        var proto = new ChessRogueUpdateDiceInfoScNotify
        {
            RogueDiceInfo = dice.ToProto()
        };

        SetData(proto);
    }
}