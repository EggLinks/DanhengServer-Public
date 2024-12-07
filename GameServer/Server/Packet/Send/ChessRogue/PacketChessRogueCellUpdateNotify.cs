using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueCellUpdateNotify : BasePacket
{
    public PacketChessRogueCellUpdateNotify(ChessRogueCellInstance cell, int boardId,
        RogueModifierSourceType source = RogueModifierSourceType.RogueModifierSourceNone,
        ChessRogueCellUpdateReason reason = ChessRogueCellUpdateReason.None) : base(
        CmdIds.ChessRogueCellUpdateNotify)
    {
        var proto = new ChessRogueCellUpdateNotify
        {
            BoardId = (uint)boardId,
            ModifierSource = source,
            Reason = reason
        };

        proto.CellList.Add(cell.ToProto());

        SetData(proto);
    }


    public PacketChessRogueCellUpdateNotify(List<ChessRogueCellInstance> cells, int boardId,
        RogueModifierSourceType source = RogueModifierSourceType.RogueModifierSourceNone,
        ChessRogueCellUpdateReason reason = ChessRogueCellUpdateReason.None) : base(
        CmdIds.ChessRogueCellUpdateNotify)
    {
        var proto = new ChessRogueCellUpdateNotify
        {
            BoardId = (uint)boardId,
            ModifierSource = source,
            Reason = reason
        };

        foreach (var cell in cells)
            proto.CellList.Add(cell.ToProto());

        SetData(proto);
    }
}