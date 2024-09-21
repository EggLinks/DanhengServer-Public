using EggLink.DanhengServer.GameServer.Game.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueEnterCellScRsp : BasePacket
{
    public PacketChessRogueEnterCellScRsp(uint cellId, ChessRogueInstance rogue) : base(CmdIds.ChessRogueEnterCellScRsp)
    {
        var proto = new ChessRogueEnterCellScRsp
        {
            CellId = cellId,
            RogueCurrentInfo = rogue.ToCurrentProto(),
            StageInfo = rogue.ToStageProto(),
            Info = rogue.ToRogueGameInfo()
        };

        SetData(proto);
    }
}