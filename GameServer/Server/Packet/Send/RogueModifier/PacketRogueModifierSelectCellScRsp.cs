using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueModifier;

public class PacketRogueModifierSelectCellScRsp : BasePacket
{
    public PacketRogueModifierSelectCellScRsp(uint cellId) : base(CmdIds.RogueModifierSelectCellScRsp)
    {
        var proto = new RogueModifierSelectCellScRsp
        {
            CellId = cellId
        };

        SetData(proto);
    }
}