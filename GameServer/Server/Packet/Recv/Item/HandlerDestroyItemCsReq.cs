using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.DestroyItemCsReq)]
public class HandlerDestroyItemCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = DestroyItemCsReq.Parser.ParseFrom(data);

        await connection.Player!.InventoryManager!.RemoveItem((int)req.ItemCount, (int)req.ItemCount);
        await connection.SendPacket(CmdIds.DestroyItemScRsp);
    }
}