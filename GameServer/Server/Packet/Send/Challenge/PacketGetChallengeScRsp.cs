using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;

public class PacketGetChallengeScRsp : BasePacket
{
    public PacketGetChallengeScRsp(PlayerInstance player) : base(CmdIds.GetChallengeScRsp)
    {
        var proto = new GetChallengeScRsp
        {
            Retcode = 0
        };

        foreach (var challengeExcel in GameData.ChallengeConfigData.Values)
            if (player.ChallengeManager?.ChallengeData.History.TryGetValue(challengeExcel.ID, out var value) == true)
            {
                var history = value;
                proto.ChallengeList.Add(history.ToProto());
            }
            else
            {
                proto.ChallengeList.Add(new Proto.Challenge
                {
                    ChallengeId = (uint)challengeExcel.ID
                });
            }

        foreach (var reward in player.ChallengeManager?.ChallengeData?.TakenRewards.Values.ToList() ?? [])
            proto.ChallengeGroupList.Add(reward.ToProto());

        SetData(proto);
    }
}