using EggLink.DanhengServer.GameServer.Server.Packet.Send.RollShop;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RollShop;

[Opcode(CmdIds.GetRollShopInfoCsReq)]
public class HandlerGetRollShopInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetRollShopInfoCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(new PacketGetRollShopInfoScRsp(req.RollShopId));
    }
}