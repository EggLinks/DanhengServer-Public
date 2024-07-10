using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission
{
    public class PacketUpdateTrackMainMissionIdScRsp : BasePacket
    {
        public PacketUpdateTrackMainMissionIdScRsp(int prev, int cur) : base(CmdIds.UpdateTrackMainMissionIdScRsp)
        {
            var proto = new UpdateTrackMainMissionIdScRsp()
            {
                PrevTrackMissionId = (uint)prev,
                TrackMissionId = (uint)cur
            };

            SetData(proto);
        }
    }
}
