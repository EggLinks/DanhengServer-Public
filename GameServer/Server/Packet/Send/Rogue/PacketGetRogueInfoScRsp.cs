using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketGetRogueInfoScRsp : BasePacket
{
    public PacketGetRogueInfoScRsp(PlayerInstance player) : base(CmdIds.GetRogueInfoScRsp)
    {
        var proto = new GetRogueInfoScRsp
        {
            RogueGameInfo = player.RogueManager!.ToProto()
        };

        SetData(proto);
    }
}