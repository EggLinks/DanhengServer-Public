using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;

public class PacketGetAssistHistoryScRsp : BasePacket
{
    public PacketGetAssistHistoryScRsp(PlayerInstance player) : base(CmdIds.GetAssistHistoryScRsp)
    {
        var proto = new GetAssistHistoryScRsp();

        SetData(proto);
    }
}