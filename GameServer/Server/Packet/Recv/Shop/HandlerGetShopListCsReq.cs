using EggLink.DanhengServer.GameServer.Server.Packet.Send.Shop;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Shop;

[Opcode(CmdIds.GetShopListCsReq)]
public class HandlerGetShopListCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetShopListCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(new PacketGetShopListScRsp(req.ShopType));
    }
}