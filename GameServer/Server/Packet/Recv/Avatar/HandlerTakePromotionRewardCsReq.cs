using EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.TakePromotionRewardCsReq)]
public class HandlerTakePromotionRewardCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = TakePromotionRewardCsReq.Parser.ParseFrom(data);

        var avatar = connection.Player!.AvatarManager!.GetAvatar((int)req.BaseAvatarId);
        if (avatar == null) return;
        avatar.TakeReward((int)req.Promotion);
        await connection.Player!.InventoryManager!.AddItem(101, 1, false);
        await connection.SendPacket(new PacketPlayerSyncScNotify(avatar));

        await connection.SendPacket(new PacketTakePromotionRewardScRsp());
    }
}