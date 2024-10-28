using EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.UseItemCsReq)]
public class HandlerUseItemCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = UseItemCsReq.Parser.ParseFrom(data);
        var result =
            await connection.Player!.InventoryManager!.UseItem((int)req.UseItemId, (int)req.UseItemCount,
                (int)req.BaseAvatarId);

        await connection.SendPacket(new PacketUseItemScRsp(result.Item1, req.UseItemId, req.UseItemCount,
            result.returnItems));
    }
}