using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;

public class PacketGetCurLineupDataScRsp : BasePacket
{
    public PacketGetCurLineupDataScRsp(PlayerInstance player) : base(CmdIds.GetCurLineupDataScRsp)
    {
        var data = new GetCurLineupDataScRsp
        {
            Lineup = player.LineupManager?.GetCurLineup()?.ToProto() ?? new LineupInfo()
        };

        SetData(data);
    }
}