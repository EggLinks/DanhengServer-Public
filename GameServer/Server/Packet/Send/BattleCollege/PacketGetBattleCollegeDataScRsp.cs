using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.BattleCollege;

public class PacketGetBattleCollegeDataScRsp : BasePacket
{
    public PacketGetBattleCollegeDataScRsp(PlayerInstance player) : base(CmdIds.GetBattleCollegeDataScRsp)
    {
        var proto = new GetBattleCollegeDataScRsp();

        foreach (var id in player.BattleCollegeData?.FinishedCollegeIdList ?? [])
            proto.FinishedCollegeIdList.Add((uint)id);

        SetData(proto);
    }
}