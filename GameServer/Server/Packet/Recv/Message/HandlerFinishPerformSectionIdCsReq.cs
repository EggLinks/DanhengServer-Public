using EggLink.DanhengServer.GameServer.Server.Packet.Send.Message;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Message;

[Opcode(CmdIds.FinishPerformSectionIdCsReq)]
public class HandlerFinishPerformSectionIdCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = FinishPerformSectionIdCsReq.Parser.ParseFrom(data);

        await connection.Player!.MessageManager!.FinishSection((int)req.SectionId);

        await connection.SendPacket(new PacketFinishPerformSectionIdScRsp(req.SectionId));
    }
}