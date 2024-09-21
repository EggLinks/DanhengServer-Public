using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketQuitRogueScRsp : BasePacket
{
    public PacketQuitRogueScRsp(PlayerInstance player) : base(CmdIds.QuitRogueScRsp)
    {
        var proto = new QuitRogueScRsp
        {
            RogueGameInfo = player.RogueManager!.ToProto()
        };

        SetData(proto);
    }
}