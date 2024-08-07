using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketPlayerHeartBeatScRsp : BasePacket
{
    public PacketPlayerHeartBeatScRsp(long clientTime) : base(CmdIds.PlayerHeartBeatScRsp)
    {
        var data = new PlayerHeartBeatScRsp
        {
            ClientTimeMs = (ulong)clientTime,
            ServerTimeMs = (ulong)Extensions.GetUnixMs()
        };

        SetData(data);
    }
}