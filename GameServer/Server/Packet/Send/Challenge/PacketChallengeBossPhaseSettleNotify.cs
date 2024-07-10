using EggLink.DanhengServer.Game.Challenge;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge
{
    public class PacketChallengeBossPhaseSettleNotify : BasePacket
    {
        public PacketChallengeBossPhaseSettleNotify(ChallengeInstance challenge) : base(CmdIds.ChallengeBossPhaseSettleNotify)
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
                HCLKAEHJCDO = true,
            };

            SetData(proto);
        }
    }
}
