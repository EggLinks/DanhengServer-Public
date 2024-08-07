using EggLink.DanhengServer.GameServer.Game.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueQuitScRsp : BasePacket
{
    public PacketChessRogueQuitScRsp(ChessRogueInstance instance) : base(CmdIds.ChessRogueQuitScRsp)
    {
        var proto = new ChessRogueQuitScRsp
        {
            FinishInfo = instance.ToFinishInfo(),
            Info = instance.ToProto(),
            LevelInfo = instance.ToLevelInfo(),
            PlayerInfo = instance.ToPlayerProto(),
            QueryInfo = instance.Player.ChessRogueManager!.ToQueryInfo(),
            RogueGetInfo = instance.Player.ChessRogueManager!.ToGetInfo(),
            RogueAeonInfo = instance.Player.ChessRogueManager!.ToRogueAeonInfo()
        };

        SetData(proto);
    }
}