using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueUpdateActionPointScNotify : BasePacket
{
    public PacketChessRogueUpdateActionPointScNotify(int actionPoint) : base(CmdIds.ChessRogueUpdateActionPointScNotify)
    {
        var proto = new ChessRogueUpdateActionPointScNotify
        {
            ActionPoint = actionPoint
        };

        SetData(proto);
    }
}