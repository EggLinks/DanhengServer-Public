using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;

public class PacketGetAllLineupDataScRsp : BasePacket
{
    public PacketGetAllLineupDataScRsp(PlayerInstance player) : base(CmdIds.GetAllLineupDataScRsp)
    {
        var proto = new GetAllLineupDataScRsp
        {
            CurIndex = (uint)player.LineupManager!.LineupData.CurLineup
        };
        foreach (var lineup in player.LineupManager.GetAllLineup()) proto.LineupList.Add(lineup.ToProto());

        SetData(proto);
    }
}