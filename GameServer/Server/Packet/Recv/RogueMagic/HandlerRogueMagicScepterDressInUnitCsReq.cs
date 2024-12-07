using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueMagic;

[Opcode(CmdIds.RogueMagicScepterDressInUnitCsReq)]
public class HandlerRogueMagicScepterDressInUnitCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueMagicScepterDressInUnitCsReq.Parser.ParseFrom(data);

        var rogue = connection.Player!.RogueManager?.GetRogueInstance();
        if (rogue == null || rogue is not RogueMagicInstance instance)
        {
            await connection.SendPacket(CmdIds.RogueMagicScepterDressInUnitScRsp);
            return;
        }

        await instance.DressScepter((int)req.ScepterId, (int)req.DiceSlotId, (int)req.DressMagicUnitUniqueId);
        await connection.SendPacket(CmdIds.RogueMagicScepterDressInUnitScRsp);
    }
}