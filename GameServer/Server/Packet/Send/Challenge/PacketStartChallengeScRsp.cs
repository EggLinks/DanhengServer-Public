using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Challenge
{
    public class PacketStartChallengeScRsp : BasePacket
    {
        public PacketStartChallengeScRsp(uint Retcode) : base(CmdIds.StartChallengeScRsp)
        {
            StartChallengeScRsp proto = new StartChallengeScRsp
            {
                Retcode = Retcode,
            };

            SetData(proto);
        }

        public PacketStartChallengeScRsp(PlayerInstance player) : base(CmdIds.StartChallengeScRsp)
        {
            StartChallengeScRsp proto = new StartChallengeScRsp() { };

            if (player.ChallengeManager!.ChallengeInstance != null)
            {
                proto.CurChallenge = player.ChallengeManager.ChallengeInstance.ToProto();
                proto.Lineup = player.LineupManager!.GetExtraLineup(ExtraLineupType.LineupChallenge)!.ToProto(); // Deprecated in 2.3
                
                // Early implementation for 2.3
                /* proto.LineupList.Add(player.LineupManager!.GetExtraLineup(ExtraLineupType.LineupChallenge)!.ToProto());
                proto.Lineup.Add(player.LineupManager!.GetExtraLineup(ExtraLineupType.LineupChallenge2)!.ToProto()); */
            }
            else
            {
                proto.Retcode = 1;
            }

            SetData(proto);
        }
    }
}
