using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.StartChallengeCsReq)]
    public class HandlerStartChallengeCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = StartChallengeCsReq.Parser.ParseFrom(data);

            StartChallengeStoryBuffInfo? storyBuffInfo = null;
            if (req.PlayerInfo != null && req.PlayerInfo.StoryBuffInfo != null)
            {
                storyBuffInfo = req.PlayerInfo.StoryBuffInfo;
            };

            StartChallengeBossBuffInfo? bossBuffInfo = null;
            if (req.PlayerInfo != null && req.PlayerInfo.BossBuffInfo != null)
            {
                bossBuffInfo = req.PlayerInfo.BossBuffInfo;
            };
            
            if (req.TeamOne.Count > 0)
            {
                connection.Player!.LineupManager!.ReplaceLineup(0, req.TeamOne.Select(x => (int)x).ToList(), ExtraLineupType.LineupChallenge);
            }

            if (req.TeamTwo.Count > 0)
            {
                connection.Player!.LineupManager!.ReplaceLineup(0, req.TeamTwo.Select(x => (int)x).ToList(), ExtraLineupType.LineupChallenge2);
            }

            connection.Player!.ChallengeManager!.StartChallenge((int)req.ChallengeId, storyBuffInfo, bossBuffInfo);
        }
    }
}
