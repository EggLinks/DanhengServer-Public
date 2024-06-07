using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Battle
{
    public class PacketGetCurChallengeScRsp : BasePacket
    {
        public PacketGetCurChallengeScRsp(PlayerInstance player) : base(CmdIds.GetCurChallengeScRsp)
        {
            var proto = new GetCurChallengeScRsp() { };

            if (player.ChallengeManager!.ChallengeInstance != null)
            {
                proto.CurChallenge = player.ChallengeManager.ChallengeInstance.ToProto();
                player.LineupManager!.SetCurLineup(player.ChallengeManager.ChallengeInstance.CurrentExtraLineup + 10);
            }
            else
            {
                proto.Retcode = 0;
            }

            SetData(proto);
        }
    }
}
