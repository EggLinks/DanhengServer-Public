using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.DressAvatarCsReq)]
    public class HandlerDressAvatarCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = DressAvatarCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;

            player.InventoryManager!.EquipAvatar((int)req.DressAvatarId, (int)req.EquipmentUniqueId);

            connection.SendPacket(CmdIds.DressAvatarScRsp);
        }
    }
}
