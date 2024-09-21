using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueUpdateAllowedSelectCellScNotify : BasePacket
{
    public PacketChessRogueUpdateAllowedSelectCellScNotify(int boardId, List<int> allowed) : base(
        CmdIds.ChessRogueUpdateAllowedSelectCellScNotify)
    {
        var proto = new ChessRogueUpdateAllowedSelectCellScNotify
        {
            BoardId = (uint)boardId
        };

        foreach (var cell in allowed) proto.AllowSelectCellIdList.Add((uint)cell);

        SetData(proto);
    }
}