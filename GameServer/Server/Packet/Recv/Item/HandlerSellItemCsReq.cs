using EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.SellItemCsReq)]
public class HandlerSellItemCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SellItemCsReq.Parser.ParseFrom(data);
        var items = await connection.Player!.InventoryManager!.SellItem(req.CostData);
        await connection.SendPacket(new PacketSellItemScRsp(items));
    }
}