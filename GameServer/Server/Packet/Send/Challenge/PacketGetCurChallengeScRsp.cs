using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;

public class PacketGetCurChallengeScRsp : BasePacket
{
    public PacketGetCurChallengeScRsp(PlayerInstance player) : base(CmdIds.GetCurChallengeScRsp)
    {
        var proto = new GetCurChallengeScRsp();

        if (player.ChallengeManager!.ChallengeInstance != null)
        {
            proto.CurChallenge = player.ChallengeManager.ChallengeInstance.ToProto();
            Task.Run(async () =>
            {
                await player.LineupManager!.SetCurLineup(player.ChallengeManager.ChallengeInstance
                    .CurrentExtraLineup + 10);
            }).Wait();
            var proto1 = player.LineupManager?.GetExtraLineup(ExtraLineupType.LineupChallenge)?.ToProto();
            if (proto1 != null)
                proto.LineupList.Add(proto1);

            var proto2 = player.LineupManager?.GetExtraLineup(ExtraLineupType.LineupChallenge2)?.ToProto();
            if (proto2 != null)
                proto.LineupList.Add(proto2);
        }
        else
        {
            proto.Retcode = 0;
        }

        SetData(proto);
    }
}