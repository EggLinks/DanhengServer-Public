using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketServerAnnounceNotify : BasePacket
{
    public PacketServerAnnounceNotify() : base(CmdIds.ServerAnnounceNotify)
    {
        var proto = new ServerAnnounceNotify();

        proto.AnnounceDataList.Add(new AnnounceData
        {
            BeginTime = Extensions.GetUnixSec(),
            EndTime = Extensions.GetUnixSec() + 3600,
            ConfigId = 1,
            CENCAKDHHHA = ConfigManager.Config.ServerOption.ServerAnnounce.AnnounceContent
        });

        if (ConfigManager.Config.ServerOption.ServerAnnounce.EnableAnnounce) SetData(proto);
    }
}