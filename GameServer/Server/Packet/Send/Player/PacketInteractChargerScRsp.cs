using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketInteractChargerScRsp : BasePacket
    {
        public PacketInteractChargerScRsp(ChargerInfo chargerInfo) : base(CmdIds.InteractChargerScRsp)
        {
            var proto = new InteractChargerScRsp()
            {
                ChargerInfo = chargerInfo
            };

            SetData(proto);
        }
    }
}
