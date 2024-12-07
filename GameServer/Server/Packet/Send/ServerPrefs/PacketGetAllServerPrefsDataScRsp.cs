using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ServerPrefs;

public class PacketGetAllServerPrefsDataScRsp : BasePacket
{
    public PacketGetAllServerPrefsDataScRsp(List<ServerPrefsInfo> infos) : base(CmdIds.GetAllServerPrefsDataScRsp)
    {
        var proto = new GetAllServerPrefsDataScRsp();

        foreach (var info in infos) proto.ServerPrefsList.Add(info.ToProto());

        SetData(proto);
    }
}