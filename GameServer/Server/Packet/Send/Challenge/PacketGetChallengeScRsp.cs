using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Battle
{
    public class PacketGetChallengeScRsp : BasePacket
    {
        public PacketGetChallengeScRsp(PlayerInstance player) : base(CmdIds.GetChallengeScRsp)
        {
            var proto = new GetChallengeScRsp()
            {
                Retcode = 0,
            };

            foreach (var challengeExcel in GameData.ChallengeConfigData.Values)
            {
                // Skip Apocalyptic Shadow
                if (challengeExcel.ID > 30000) continue;

                if (player.ChallengeManager!.ChallengeData.History.ContainsKey(challengeExcel.ID))
                {
                    var history = player.ChallengeManager!.ChallengeData.History[challengeExcel.ID];
                    proto.ChallengeList.Add(history.ToProto());
                }
                else
                {
                    proto.ChallengeList.Add(new Proto.Challenge()
                    {
                        ChallengeId = (uint)challengeExcel.ID,
                    });
                }
            }

            foreach (var reward in player.ChallengeManager.ChallengeData.TakenRewards.Values)
            {
                proto.ChallengeGroupList.Add(reward.ToProto());
            }

            SetData(proto);
        }
    }
}
