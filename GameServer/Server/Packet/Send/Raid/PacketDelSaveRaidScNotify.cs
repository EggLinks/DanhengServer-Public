using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid
{
    public class PacketDelSaveRaidScNotify : BasePacket
    {
        public PacketDelSaveRaidScNotify(int raidId, int worldLevel) : base(CmdIds.DelSaveRaidScNotify)
        {
            var proto = new DelSaveRaidScNotify()
            {
                RaidId = (uint)raidId,
                WorldLevel = (uint)worldLevel,
            };

            SetData(proto);
        }
    }
}
