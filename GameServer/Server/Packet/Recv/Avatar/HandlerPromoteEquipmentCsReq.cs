using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.PromoteEquipmentCsReq)]
    public class HandlerPromoteEquipmentCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = PromoteEquipmentCsReq.Parser.ParseFrom(data);

            connection.Player!.InventoryManager!.PromoteEquipment((int)req.EquipmentUniqueId);

            connection.SendPacket(CmdIds.PromoteEquipmentScRsp);
        }
    }
}
