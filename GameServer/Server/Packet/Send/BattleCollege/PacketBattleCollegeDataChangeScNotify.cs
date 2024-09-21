using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.BattleCollege;

public class PacketBattleCollegeDataChangeScNotify : BasePacket
{
    public PacketBattleCollegeDataChangeScNotify(PlayerInstance player) : base(CmdIds.BattleCollegeDataChangeScNotify)
    {
        var proto = new BattleCollegeDataChangeScNotify();

        foreach (var id in player.BattleCollegeData?.FinishedCollegeIdList ?? [])
            proto.FinishedCollegeIdList.Add((uint)id);

        SetData(proto);
    }
}