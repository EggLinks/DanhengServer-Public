using EggLink.DanhengServer.GameServer.Game.Challenge;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;

public class PacketChallengeSettleNotify : BasePacket
{
    public PacketChallengeSettleNotify(ChallengeInstance challenge) : base(CmdIds.ChallengeSettleNotify)
    {
        var proto = new ChallengeSettleNotify
        {
            ChallengeId = (uint)challenge.Excel.ID,
            IsWin = challenge.IsWin(),
            ChallengeScore = (uint)challenge.ScoreStage1,
            ScoreTwo = (uint)challenge.ScoreStage2,
            Star = (uint)challenge.Stars,
            Reward = new ItemList()
        };

        SetData(proto);
    }
}