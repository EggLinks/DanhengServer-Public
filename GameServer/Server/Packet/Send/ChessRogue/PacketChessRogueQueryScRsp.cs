using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueQueryScRsp : BasePacket
{
    public PacketChessRogueQueryScRsp(PlayerInstance player) : base(CmdIds.ChessRogueQueryScRsp)
    {
        var proto = new ChessRogueQueryScRsp
        {
            RogueGetInfo = player.ChessRogueManager!.ToGetInfo(),
            Info = player.ChessRogueManager!.ToGameInfo(),
            QueryInfo = player.ChessRogueManager!.ToQueryInfo()
        };

        SetData(proto);
    }
}