using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ServerPrefs;

public class PacketUpdateServerPrefsDataScRsp : BasePacket
{
    public PacketUpdateServerPrefsDataScRsp(uint prefsId) : base(CmdIds.UpdateServerPrefsDataScRsp)
    {
        var proto = new UpdateServerPrefsDataScRsp
        {
            ServerPrefsId = prefsId
        };

        SetData(proto);
    }
}