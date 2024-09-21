using EggLink.DanhengServer.GameServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueCheatRollScRsp : BasePacket
{
    public PacketChessRogueCheatRollScRsp(ChessRogueDiceInstance dice, int surfaceId) : base(
        CmdIds.ChessRogueCheatRollScRsp)
    {
        var proto = new ChessRogueCheatRollScRsp
        {
            RogueDiceInfo = dice.ToProto(),
            DiceSurfaceId = (uint)surfaceId
        };

        SetData(proto);
    }
}