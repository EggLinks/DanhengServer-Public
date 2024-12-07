using EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;

public class PacketRogueModifierAddNotify : BasePacket
{
    public PacketRogueModifierAddNotify(ChessRogueDiceModifierInstance modifier) : base(CmdIds.RogueModifierAddNotify)
    {
        var proto = new RogueModifierAddNotify
        {
            Modifier = modifier.ToProto()
        };

        SetData(proto);
    }
}