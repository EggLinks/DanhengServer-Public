using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketPlayerLoginScRsp : BasePacket
{
    public PacketPlayerLoginScRsp(Connection connection) : base(CmdIds.PlayerLoginScRsp)
    {
        var rsp = new PlayerLoginScRsp
        {
            CurTimezone = (int)TimeZoneInfo.Local.BaseUtcOffset.TotalHours,
            ServerTimestampMs = (ulong)Extensions.GetUnixMs(),
            BasicInfo = connection.Player?.ToProto(), // should not be null
            Stamina = (uint)(connection.Player?.Data.Stamina ?? 0)
        };

        SetData(rsp);
    }
}