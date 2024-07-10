using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid
{
    public class PacketRaidInfoNotify : BasePacket
    {
        public PacketRaidInfoNotify(RaidRecord record) : base(CmdIds.RaidInfoNotify)
        {
            var proto = new RaidInfoNotify()
            {
                RaidId = (uint)record.RaidId,
                Status = record.Status,
                WorldLevel = (uint)record.WorldLevel,
                RaidFinishTime = (ulong)record.FinishTimeStamp,
                TargetInfo = { },
                ItemList = new(),
            };

            SetData(proto);
        }
    }
}
