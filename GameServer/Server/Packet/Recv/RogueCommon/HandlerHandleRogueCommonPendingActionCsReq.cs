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

        if (req.BuffSelectResult != null) await rogue.HandleBuffSelect((int)req.BuffSelectResult.BuffSelectId);

        if (req.BuffRerollSelectResult != null) await rogue.HandleRerollBuff();

        if (req.BonusSelectResult != null) await rogue.HandleBonusSelect((int)req.BonusSelectResult.BonusId);

        if (req.MiracleSelectResult != null) await rogue.HandleMiracleSelect(req.MiracleSelectResult.MiracleSelectId);
    }
}