using EggLink.DanhengServer.GameServer.Server.Packet.Send.Quest;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Quest;

[Opcode(CmdIds.FinishQuestCsReq)]
public class HandlerFinishQuestCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = FinishQuestCsReq.Parser.ParseFrom(data);
        var retCode = await connection.Player!.QuestManager!.FinishQuestByClient((int)req.QuestId);
        await connection.SendPacket(new PacketFinishQuestScRsp(retCode));
    }
}