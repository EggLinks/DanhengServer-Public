using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Rogue;

[Opcode(CmdIds.RogueNpcDisappearCsReq)]
public class HandlerRogueNpcDisappearCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueNpcDisappearCsReq.Parser.ParseFrom(data);

        if (connection.Player!.RogueManager?.GetRogueInstance() == null)
        {
            await connection.SendPacket(CmdIds.RogueNpcDisappearScRsp);
            return;
        }

        await connection.Player!.RogueManager!.GetRogueInstance()!.HandleNpcDisappear((int)req.DisappearNpcEntityId);

        await connection.SendPacket(CmdIds.RogueNpcDisappearScRsp);
    }
}