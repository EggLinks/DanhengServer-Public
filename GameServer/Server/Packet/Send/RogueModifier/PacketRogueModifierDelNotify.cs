using EggLink.DanhengServer.GameServer.Game.ChessRogue.Modifier;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;

public class PacketRogueModifierDelNotify : BasePacket
{
    public PacketRogueModifierDelNotify(ChessRogueDiceModifierInstance modifier) : base(CmdIds.RogueModifierDelNotify)
    {
        var proto = new RogueModifierDelNotify
        {
            ModifierId = (ulong)modifier.ModifierId
        };

        SetData(proto);
    }
}