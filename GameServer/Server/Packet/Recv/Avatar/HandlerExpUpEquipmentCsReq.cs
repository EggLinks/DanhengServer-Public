using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.ExpUpEquipmentCsReq)]
    public class HandlerExpUpEquipmentCsReq : Handler
    { 
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ExpUpEquipmentCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            var returnItem = player.InventoryManager!.LevelUpEquipment((int)req.EquipmentUniqueId, req.CostData);

            connection.SendPacket(new PacketExpUpEquipmentScRsp(returnItem));
        }
    }
}
