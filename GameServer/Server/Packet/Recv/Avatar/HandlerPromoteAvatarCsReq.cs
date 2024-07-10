using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.PromoteAvatarCsReq)]
    public class HandlerPromoteAvatarCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = PromoteAvatarCsReq.Parser.ParseFrom(data);

            connection.Player!.InventoryManager!.promoteAvatar((int)req.BaseAvatarId);

            connection.SendPacket(CmdIds.PromoteAvatarScRsp);
        }
    }
}
