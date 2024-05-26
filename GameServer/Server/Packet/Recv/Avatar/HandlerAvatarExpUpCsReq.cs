using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.AvatarExpUpCsReq)]
    public class HandlerAvatarExpUpCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = AvatarExpUpCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            var returnItem = player.InventoryManager!.LevelUpAvatar((int)req.BaseAvatarId, req.ItemCost);

            connection.SendPacket(new PacketAvatarExpUpScRsp(returnItem));
        }
    }
}
