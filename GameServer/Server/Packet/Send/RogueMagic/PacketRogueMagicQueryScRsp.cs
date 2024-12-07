using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;

public class PacketRogueMagicQueryScRsp : BasePacket
{
    public PacketRogueMagicQueryScRsp(PlayerInstance player) : base(CmdIds.RogueMagicQueryScRsp)
    {
        var proto = new RogueMagicQueryScRsp
        {
            RogueGetInfo = player.RogueMagicManager!.ToGetInfo()
        };

        SetData(proto);
    }
}