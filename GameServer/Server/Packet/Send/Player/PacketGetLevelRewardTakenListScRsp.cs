using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketGetLevelRewardTakenListScRsp : BasePacket
{
    public PacketGetLevelRewardTakenListScRsp(PlayerInstance player) : base(CmdIds.GetLevelRewardTakenListScRsp)
    {
        var proto = new GetLevelRewardTakenListScRsp
        {
            LevelRewardTakenList = { player.Data.TakenLevelReward.Select(x => (uint)x) }
        };

        SetData(proto);
    }
}