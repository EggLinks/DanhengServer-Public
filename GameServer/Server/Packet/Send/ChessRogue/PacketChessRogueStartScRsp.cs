using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueStartScRsp : BasePacket
{
    public PacketChessRogueStartScRsp(PlayerInstance player) : base(CmdIds.ChessRogueStartScRsp)
    {
        var proto = new ChessRogueStartScRsp
        {
            StageInfo = player.ChessRogueManager!.RogueInstance!.ToStageProto(),
            RogueCurrentInfo = player.ChessRogueManager!.RogueInstance!.ToCurrentProto(),
            Info = player.ChessRogueManager!.RogueInstance!.ToRogueGameInfo()
        };

        SetData(proto);
    }
}