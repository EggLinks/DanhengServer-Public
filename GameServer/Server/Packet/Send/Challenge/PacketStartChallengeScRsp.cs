using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;

public class PacketStartChallengeScRsp : BasePacket
{
    public PacketStartChallengeScRsp(uint Retcode) : base(CmdIds.StartChallengeScRsp)
    {
        var proto = new StartChallengeScRsp
        {
            Retcode = Retcode
        };

        SetData(proto);
    }

    public PacketStartChallengeScRsp(PlayerInstance player, bool sendScene = true) : base(CmdIds.StartChallengeScRsp)
    {
        StartChallengeScRsp proto = new();

        if (player.ChallengeManager!.ChallengeInstance != null)
        {
            proto.CurChallenge = player.ChallengeManager.ChallengeInstance.ToProto();
            proto.StageInfo = player.ChallengeManager.ChallengeInstance.ToStageInfo();
            proto.LineupList.Add(player.LineupManager!.GetExtraLineup(ExtraLineupType.LineupChallenge)!.ToProto());
            proto.LineupList.Add(player.LineupManager!.GetExtraLineup(ExtraLineupType.LineupChallenge2)!.ToProto());
            if (sendScene) proto.Scene = player.SceneInstance!.ToProto();
        }
        else
        {
            proto.Retcode = 1;
        }

        SetData(proto);
    }
}