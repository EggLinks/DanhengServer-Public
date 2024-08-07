using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueUpdateLevelBaseInfoScNotify : BasePacket
{
    public PacketChessRogueUpdateLevelBaseInfoScNotify(ChessRogueLevelStatus status) : base(
        CmdIds.ChessRogueUpdateLevelBaseInfoScNotify)
    {
        var proto = new ChessRogueUpdateLevelBaseInfoScNotify
        {
            LevelStatus = status
        };

        SetData(proto);
    }
}