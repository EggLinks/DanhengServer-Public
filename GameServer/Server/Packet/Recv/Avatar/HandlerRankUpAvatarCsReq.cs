using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.RankUpAvatarCsReq)]
public class HandlerRankUpAvatarCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RankUpAvatarCsReq.Parser.ParseFrom(data);
        await connection.Player!.InventoryManager!.RankUpAvatar((int)req.AvatarId, req.CostData);
        await connection.SendPacket(CmdIds.RankUpAvatarScRsp);
    }
}