using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.UnlockSkilltreeCsReq)]
public class HandlerUnlockSkilltreeCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = UnlockSkilltreeCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        GameData.AvatarSkillTreeConfigData.TryGetValue((int)(req.PointId * 10 + req.Level), out var config);
        if (config == null)
        {
            await connection.SendPacket(new PacketUnlockSkilltreeScRsp(Retcode.RetSkilltreeConfigNotExist));
            return;
        }

        var avatar = player.AvatarManager!.GetAvatar(config.AvatarID);
        if (avatar == null)
        {
            await connection.SendPacket(new PacketUnlockSkilltreeScRsp(Retcode.RetAvatarNotExist));
            return;
        }

        foreach (var cost in req.ItemList)
            await connection.Player!.InventoryManager!.RemoveItem((int)cost.PileItem.ItemId,
                (int)cost.PileItem.ItemNum);

        avatar.GetSkillTree()[(int)req.PointId] = (int)req.Level;

        await connection.SendPacket(new PacketPlayerSyncScNotify(avatar));

        player.MissionManager?.HandleFinishType(MissionFinishTypeEnum.UnlockSkilltreeCnt, "UnlockSkillTree");
        player.MissionManager?.HandleFinishType(MissionFinishTypeEnum.UnlockSkilltree, "UnlockSkillTree");
        player.MissionManager?.HandleFinishType(MissionFinishTypeEnum.AllAvatarUnlockSkilltreeCnt, "UnlockSkillTree");

        await connection.SendPacket(new PacketUnlockSkilltreeScRsp(req.PointId, req.Level));
    }
}