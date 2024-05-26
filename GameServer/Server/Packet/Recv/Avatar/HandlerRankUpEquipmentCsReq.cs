using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.RankUpEquipmentCsReq)]
    public class HandlerRankUpEquipmentCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = RankUpEquipmentCsReq.Parser.ParseFrom(data);
            connection.Player!.InventoryManager?.RankUpEquipment((int)req.EquipmentUniqueId, req.CostData);
            connection.SendPacket(CmdIds.RankUpEquipmentScRsp);
        }
    }
}
