using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueCellUpdateNotify : BasePacket
{
    public PacketChessRogueCellUpdateNotify(ChessRogueCellInstance cell, int boardId) : base(
        CmdIds.ChessRogueCellUpdateNotify)
    {
        var proto = new ChessRogueCellUpdateNotify
        {
            BoardId = (uint)boardId
        };

        proto.CellList.Add(cell.ToProto());

        SetData(proto);
    }
}