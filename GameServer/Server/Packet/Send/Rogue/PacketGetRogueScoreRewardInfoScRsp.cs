using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketGetRogueScoreRewardInfoScRsp : BasePacket
{
    public PacketGetRogueScoreRewardInfoScRsp(PlayerInstance player) : base(CmdIds.GetRogueScoreRewardInfoScRsp)
    {
        var proto = new GetRogueScoreRewardInfoScRsp
        {
            Info = player.RogueManager!.ToRewardProto()
        };

        SetData(proto);
    }
}