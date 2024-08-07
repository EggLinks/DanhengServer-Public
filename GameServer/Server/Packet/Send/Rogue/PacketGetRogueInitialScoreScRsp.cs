using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketGetRogueInitialScoreScRsp : BasePacket
{
    public PacketGetRogueInitialScoreScRsp(PlayerInstance player) : base(CmdIds.GetRogueInitialScoreScRsp)
    {
        var proto = new GetRogueInitialScoreScRsp
        {
            RogueScoreRewardInfo = player.RogueManager!.ToRewardProto()
        };

        SetData(proto);
    }
}