using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.UnlockSkilltreeCsReq)]
    public class HandlerUnlockSkilltreeCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = UnlockSkilltreeCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            GameData.AvatarSkillTreeConfigData.TryGetValue((int)(req.PointId * 10 + req.Level), out var config);
            if (config == null)
            {
                connection.SendPacket(new PacketUnlockSkilltreeScRsp());
                return;
            }
            var avatar = player.AvatarManager!.GetAvatar(config.AvatarID);
            if (avatar == null)
            {
                connection.SendPacket(new PacketUnlockSkilltreeScRsp());
                return;
            }
            foreach (var cost in req.ItemList)
            {
                connection.Player!.InventoryManager!.RemoveItem((int)cost.PileItem.ItemId, (int)cost.PileItem.ItemNum);
            }

            avatar.GetSkillTree().TryGetValue((int)req.PointId, out var level);
            avatar.GetSkillTree()[(int)req.PointId] = level + 1;
            DatabaseHelper.Instance!.UpdateInstance(player.AvatarManager.AvatarData!);

            connection.SendPacket(new PacketPlayerSyncScNotify(avatar));
            connection.SendPacket(new PacketUnlockSkilltreeScRsp((uint)avatar.AvatarId, req.PointId, req.Level));
        }
    }
}
