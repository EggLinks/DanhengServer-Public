using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueCommon;

[Opcode(CmdIds.SelectRogueCommonDialogueOptionCsReq)]
public class HandlerSelectRogueCommonDialogueOptionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SelectRogueCommonDialogueOptionCsReq.Parser.ParseFrom(data);

        var rogue = connection.Player!.RogueManager?.GetRogueInstance();
        if (rogue == null) return;

        await rogue.HandleSelectOption((int)req.EventUniqueId, (int)req.OptionId);
    }
}