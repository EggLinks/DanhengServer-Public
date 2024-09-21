using EggLink.DanhengServer.GameServer.Game.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueEnterNextLayerScRsp : BasePacket
{
    public PacketChessRogueEnterNextLayerScRsp(ChessRogueInstance rogue) : base(CmdIds.ChessRogueEnterNextLayerScRsp)
    {
        var proto = new ChessRogueEnterNextLayerScRsp
        {
            StageInfo = rogue.ToStageProto(),
            RogueGameInfo = rogue.ToRogueGameInfo(),
            RogueCurrentInfo = rogue.ToCurrentProto()
        };

        SetData(proto);
    }
}