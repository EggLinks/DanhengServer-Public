using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketGetChessRogueBuffEnhanceInfoScRsp : BasePacket
{
    public PacketGetChessRogueBuffEnhanceInfoScRsp(PlayerInstance player) : base(CmdIds
        .GetChessRogueBuffEnhanceInfoScRsp)
    {
        var proto = new GetChessRogueBuffEnhanceInfoScRsp();
        if (player.ChessRogueManager!.RogueInstance == null)
        {
            proto.Retcode = 1;
            SetData(proto);
            return;
        }

        proto.BuffEnhanceInfo = player.ChessRogueManager.RogueInstance!.ToChessEnhanceInfo();

        SetData(proto);
    }
}