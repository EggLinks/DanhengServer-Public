using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Others
{
    public class PacketServerAnnounceNotify : BasePacket
    {
        public PacketServerAnnounceNotify() : base(CmdIds.ServerAnnounceNotify)
        {
            var proto = new ServerAnnounceNotify();

            proto.AnnounceDataList.Add(new AnnounceData()
            {
                BeginTime = Extensions.GetUnixSec(),
                EndTime = Extensions.GetUnixSec() + 3600,
                ConfigId = 1,
                CHJOJJLOBEI = ConfigManager.Config.ServerOption.ServerAnnounce.AnnounceContent,
            });

            if (ConfigManager.Config.ServerOption.ServerAnnounce.EnableAnnounce)
            {
                SetData(proto);
            }
        }
    }
}
