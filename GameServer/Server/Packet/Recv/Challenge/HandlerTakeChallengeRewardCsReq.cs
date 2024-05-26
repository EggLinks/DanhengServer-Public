using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Challenge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Challenge
{
    [Opcode(CmdIds.TakeChallengeRewardCsReq)]
    public class HandlerTakeChallengeRewardCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = TakeChallengeRewardCsReq.Parser.ParseFrom(data);

            List<TakenChallengeRewardInfo>? rewardInfos = connection.Player!.ChallengeManager!.TakeRewards((int)req.GroupId)!;
            connection.SendPacket(new PacketTakeChallengeRewardScRsp((int)req.GroupId, rewardInfos));
        }
    }
}
