using EggLink.DanhengServer.GameServer.Server.Packet.Send.Shop;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Shop;

[Opcode(CmdIds.BuyGoodsCsReq)]
public class HandlerBuyGoodsCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = BuyGoodsCsReq.Parser.ParseFrom(data);
        var items = await player.ShopService!.BuyItem((int)req.ShopId, (int)req.GoodsId, (int)req.GoodsNum);

        await connection.SendPacket(new PacketBuyGoodsScRsp(req, items));
    }
}