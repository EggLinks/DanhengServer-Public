using EggLink.DanhengServer.GameServer.Game.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueLeaveScRsp : BasePacket
{
    public PacketChessRogueLeaveScRsp(ChessRogueInstance instance) : base(CmdIds.ChessRogueLeaveScRsp)
    {
        var proto = new ChessRogueLeaveScRsp
        {
            StageInfo = instance.ToStageProto(),
            QueryInfo = instance.Player.ChessRogueManager!.ToQueryInfo(),
            RogueAeonInfo = instance.Player.ChessRogueManager!.ToRogueAeonInfo(),
            RogueGetInfo = instance.Player.ChessRogueManager!.ToGetInfo()
        };

        SetData(proto);
    }
}