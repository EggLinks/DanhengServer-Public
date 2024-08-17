using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;

public class PacketRogueTournQueryScRsp : BasePacket
{
    public PacketRogueTournQueryScRsp(PlayerInstance player) : base(CmdIds.RogueTournQueryScRsp)
    {
        var proto = new RogueTournQueryScRsp
        {
            RogueGetInfo = player.RogueTournManager!.ToProto()
        };

        SetData(proto);
    }
}