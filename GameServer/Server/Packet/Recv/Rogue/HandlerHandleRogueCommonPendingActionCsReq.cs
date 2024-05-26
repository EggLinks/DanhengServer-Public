using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using Google.Protobuf;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.HandleRogueCommonPendingActionCsReq)]
    public class HandlerHandleRogueCommonPendingActionCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = HandleRogueCommonPendingActionCsReq.Parser.ParseFrom(data);

            var rogue = connection.Player!.RogueManager?.GetRogueInstance();
            if (rogue == null) return;

            if (req.BuffSelectResult != null)
            {
                rogue.HandleBuffSelect((int)req.BuffSelectResult.BuffId);
            }

            if (req.BuffRerollSelectResult != null)
            {
                rogue.HandleRerollBuff();
            }

            if (req.BonusSelectResult != null)
            {
                rogue.HandleBonusSelect((int)req.BonusSelectResult.BonusId);
            }

            if (req.MiracleSelectResult != null)
            {
                rogue.HandleMiracleSelect(req.MiracleSelectResult.MiracleId);
            }
        }
    }
}
