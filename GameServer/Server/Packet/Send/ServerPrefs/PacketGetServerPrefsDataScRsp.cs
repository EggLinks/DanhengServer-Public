using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using Google.Protobuf;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ServerPrefs;

public class PacketGetServerPrefsDataScRsp : BasePacket
{
    public PacketGetServerPrefsDataScRsp(ServerPrefsInfo? info, uint id) : base(CmdIds.GetServerPrefsDataScRsp)
    {
        var proto = new GetServerPrefsDataScRsp
        {
            ServerPrefs = info?.ToProto() ?? new Proto.ServerPrefs
            {
                Data = ByteString.Empty,
                ServerPrefsId = id
            }
        };

        SetData(proto);
    }
}