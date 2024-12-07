using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;

public class PacketRogueModifierStageStartNotify : BasePacket
{
    public PacketRogueModifierStageStartNotify(RogueModifierSourceType source) : base(
        CmdIds.RogueModifierStageStartNotify)
    {
        var proto = new RogueModifierStageStartNotify
        {
            ModifierSourceType = source
        };

        SetData(proto);
    }
}