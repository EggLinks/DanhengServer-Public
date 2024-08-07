using EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.ExpUpRelicCsReq)]
public class HandlerExpUpRelicCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ExpUpRelicCsReq.Parser.ParseFrom(data);

        var left = await connection.Player!.InventoryManager!.LevelUpRelic((int)req.RelicUniqueId, req.CostData);

        await connection.SendPacket(new PacketExpUpRelicScRsp(left));
    }
}