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

        if (req.DifficultyIdList != null)
            foreach (var difficultyId in req.DifficultyIdList)
                difficultyIdList.Add((int)difficultyId);

        if (req.DisableAeonIdList != null)
            foreach (var disableAeonId in req.DisableAeonIdList)
                disableAeonIdList.Add((int)disableAeonId);

        await player.ChessRogueManager!.StartRogue((int)req.AeonId, [.. req.BaseAvatarIdList], (int)req.Id,
            (int)req.DiceBranchId, difficultyIdList, disableAeonIdList);
    }
}