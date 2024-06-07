using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.DressRelicAvatarCsReq)]
    public class HandlerDressRelicAvatarCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = DressRelicAvatarCsReq.Parser.ParseFrom(data);

            foreach (var param in req.SwitchList)
            {
                connection.Player!.InventoryManager!.EquipRelic((int)req.DressAvatarId, (int)param.RelicUniqueId, (int) param.RelicType);
            }

            connection.SendPacket(CmdIds.DressRelicAvatarScRsp);
        }
    }
}
