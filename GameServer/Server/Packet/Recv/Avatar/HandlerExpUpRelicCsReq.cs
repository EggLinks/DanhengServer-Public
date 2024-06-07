using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.ExpUpRelicCsReq)]
    public class HandlerExpUpRelicCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ExpUpRelicCsReq.Parser.ParseFrom(data);

            var left = connection.Player!.InventoryManager!.LevelUpRelic((int)req.RelicUniqueId, req.CostData);

            connection.SendPacket(new PacketExpUpRelicScRsp(left));
        }
    }
}
