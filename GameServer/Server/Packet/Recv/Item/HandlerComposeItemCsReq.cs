using EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.ComposeItemCsReq)]
public class HandlerComposeItemCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ComposeItemCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        var item = await player.InventoryManager!.ComposeItem((int)req.ComposeId, (int)req.Count);
        if (item == null)
        {
            await connection.SendPacket(new PacketComposeItemScRsp());
            return;
        }

        await connection.SendPacket(new PacketComposeItemScRsp(req.ComposeId, req.Count, item));
    }
}