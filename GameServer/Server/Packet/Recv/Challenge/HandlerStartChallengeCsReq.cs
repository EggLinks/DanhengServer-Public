using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Challenge;

[Opcode(CmdIds.StartChallengeCsReq)]
public class HandlerStartChallengeCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = StartChallengeCsReq.Parser.ParseFrom(data);

        ChallengeStoryBuffInfo? storyBuffInfo = null;
        if (req.PlayerInfo != null && req.PlayerInfo.StoryBuffInfo != null)
            storyBuffInfo = req.PlayerInfo.StoryBuffInfo;

        ChallengeBossBuffInfo? bossBuffInfo = null;
        if (req.PlayerInfo != null && req.PlayerInfo.BossBuffInfo != null) bossBuffInfo = req.PlayerInfo.BossBuffInfo;

        if (req.FirstLineup.Count > 0)
            await connection.Player!.LineupManager!.ReplaceLineup(0, req.FirstLineup.Select(x => (int)x).ToList(),
                ExtraLineupType.LineupChallenge);

        if (req.SecondLineup.Count > 0)
            await connection.Player!.LineupManager!.ReplaceLineup(0, req.SecondLineup.Select(x => (int)x).ToList(),
                ExtraLineupType.LineupChallenge2);

        await connection.Player!.ChallengeManager!.StartChallenge((int)req.ChallengeId, storyBuffInfo, bossBuffInfo);
    }
}