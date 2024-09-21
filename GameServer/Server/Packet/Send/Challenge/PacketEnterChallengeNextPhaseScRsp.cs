using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Challenge;

public class PacketEnterChallengeNextPhaseScRsp : BasePacket
{
    public PacketEnterChallengeNextPhaseScRsp(PlayerInstance instance) : base(CmdIds.EnterChallengeNextPhaseScRsp)
    {
        var proto = new EnterChallengeNextPhaseScRsp
        {
            Scene = instance.SceneInstance!.ToProto()
        };

        SetData(proto);
    }

    public PacketEnterChallengeNextPhaseScRsp(Retcode code) : base(CmdIds.EnterChallengeNextPhaseScRsp)
    {
        var proto = new EnterChallengeNextPhaseScRsp
        {
            Retcode = (uint)code
        };

        SetData(proto);
    }
}