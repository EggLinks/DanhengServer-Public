using EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.AvatarExpUpCsReq)]
public class HandlerAvatarExpUpCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = AvatarExpUpCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        var returnItem = await player.InventoryManager!.LevelUpAvatar((int)req.BaseAvatarId, req.ItemCost);

        await connection.SendPacket(new PacketAvatarExpUpScRsp(returnItem));
    }
}