using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.GameServer.Game.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueCommon;

[Opcode(CmdIds.HandleRogueCommonPendingActionCsReq)]
public class HandlerHandleRogueCommonPendingActionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = HandleRogueCommonPendingActionCsReq.Parser.ParseFrom(data);

        var rogue = connection.Player!.RogueManager?.GetRogueInstance();
        if (rogue == null) return;

        if (req.BuffSelectResult != null)
            await rogue.HandleBuffSelect((int)req.BuffSelectResult.BuffSelectId, (int)req.QueueLocation);

        if (req.BuffReforgeSelectResult != null)
            await rogue.HandleBuffReforgeSelect((int)req.BuffReforgeSelectResult.BuffSelectId, (int)req.QueueLocation);

        if (req.BuffRerollSelectResult != null) await rogue.HandleRerollBuff((int)req.QueueLocation);

        if (req.BonusSelectResult != null)
            await rogue.HandleBonusSelect((int)req.BonusSelectResult.BonusId, (int)req.QueueLocation);

        if (req.MiracleSelectResult != null)
            await rogue.HandleMiracleSelect(req.MiracleSelectResult.MiracleSelectId, (int)req.QueueLocation);

        if (req.RogueTournFormulaResult != null && rogue is RogueTournInstance tournInstance)
            await tournInstance.HandleFormulaSelect((int)req.RogueTournFormulaResult.TournFormulaId,
                (int)req.QueueLocation);

        if (req.MagicUnitSelectResult != null && rogue is RogueMagicInstance magic)
            await magic.HandleMagicUnitSelect(req.MagicUnitSelectResult.SelectMagicUnit, (int)req.QueueLocation);

        if (req.ScepterSelectResult != null && rogue is RogueMagicInstance magic2)
            await magic2.HandleScepterSelect(req.ScepterSelectResult.SelectScepter, (int)req.QueueLocation);
    }
}