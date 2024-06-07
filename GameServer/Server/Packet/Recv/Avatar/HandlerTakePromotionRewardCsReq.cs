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
    [Opcode(CmdIds.TakePromotionRewardCsReq)]
    public class HandlerTakePromotionRewardCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = TakePromotionRewardCsReq.Parser.ParseFrom(data);

            var avatar = connection.Player!.AvatarManager!.GetAvatar((int)req.BaseAvatarId);
            if (avatar == null)
            {
                return;
            }
            avatar.TakeReward((int)req.Promotion);
            connection.Player!.InventoryManager!.AddItem(101, 1, false);
            connection.SendPacket(new PacketPlayerSyncScNotify(avatar));

            connection.SendPacket(new PacketTakePromotionRewardScRsp());
        }
    }
}
