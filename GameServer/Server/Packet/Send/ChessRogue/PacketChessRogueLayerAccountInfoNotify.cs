using EggLink.DanhengServer.GameServer.Game.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueLayerAccountInfoNotify : BasePacket
{
    public PacketChessRogueLayerAccountInfoNotify(ChessRogueInstance rogue) : base(CmdIds
        .ChessRogueLayerAccountInfoNotify)
    {
        var proto = new ChessRogueLayerAccountInfoNotify
        {
            FinishInfo = rogue.ToFinishInfo()
            //LayerId = (uint)rogue.CurLayer,
        };

        SetData(proto);
    }
}