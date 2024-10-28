using EggLink.DanhengServer.GameServer.Game.Challenge;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;

public class PacketChallengeBossPhaseSettleNotify : BasePacket
{
    public PacketChallengeBossPhaseSettleNotify(ChallengeInstance challenge, BattleTargetList? targetLists = null) :
        base(CmdIds
            .ChallengeBossPhaseSettleNotify)
    {
        var proto = new ChallengeBossPhaseSettleNotify
        {
            ChallengeId = (uint)challenge.Excel.ID,
            IsWin = challenge.IsWin(),
            ChallengeScore = (uint)challenge.ScoreStage1,
            ScoreTwo = (uint)challenge.ScoreStage2,
            Star = (uint)challenge.Stars,
            Phase = (uint)challenge.CurrentStage,
            IsRemainingAction = true,
            IsReward = true
        };

        proto.BattleTargetList.AddRange(targetLists?.BattleTargetList_ ?? []);

        SetData(proto);
    }
}