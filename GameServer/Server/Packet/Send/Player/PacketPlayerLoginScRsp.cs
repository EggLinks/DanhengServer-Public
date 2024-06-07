using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketPlayerLoginScRsp : BasePacket
    {
        public PacketPlayerLoginScRsp(Connection connection) : base(CmdIds.PlayerLoginScRsp)
        {
            var rsp = new PlayerLoginScRsp()
            {
                CurTimezone = (int)TimeZoneInfo.Local.BaseUtcOffset.TotalHours,
                ServerTimestampMs = (ulong)Extensions.GetUnixMs(),
                BasicInfo = connection?.Player?.ToProto(),  // should not be null
                Stamina = (uint)(connection?.Player?.Data.Stamina ?? 0),
            };

            SetData(rsp);
        }
    }
}
