using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;

public class PacketSearchPlayerScRsp : BasePacket
{
    public PacketSearchPlayerScRsp() : base(CmdIds.SearchPlayerScRsp)
    {
        var proto = new SearchPlayerScRsp
        {
            Retcode = 3612
        };

        SetData(proto);
    }

    public PacketSearchPlayerScRsp(List<PlayerData> data) : base(CmdIds.SearchPlayerScRsp)
    {
        var proto = new SearchPlayerScRsp();

        proto.ResultUidList.AddRange(data.Select(x => (uint)x.Uid));
        proto.SimpleInfoList.AddRange(data.Select(x => x.ToSimpleProto(FriendOnlineStatus.Online)));

        SetData(proto);
    }
}