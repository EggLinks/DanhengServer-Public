using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ChessRogue;

[Opcode(CmdIds.ChessRogueStartCsReq)]
public class HandlerChessRogueStartCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = ChessRogueStartCsReq.Parser.ParseFrom(data);

        var difficultyIdList = new List<int>();
        var disableAeonIdList = new List<int>();

        if (req.StartDifficultyIdList != null)
            difficultyIdList.AddRange(req.StartDifficultyIdList.Select(difficultyId => (int)difficultyId));

        if (req.DisableAeonIdList != null)
            disableAeonIdList.AddRange(req.DisableAeonIdList.Select(disableAeonId => (int)disableAeonId));

        await player.ChessRogueManager!.StartRogue((int)req.AeonId, [.. req.BaseAvatarIdList], (int)req.Id,
            (int)req.BranchId, difficultyIdList, disableAeonIdList);
    }
}