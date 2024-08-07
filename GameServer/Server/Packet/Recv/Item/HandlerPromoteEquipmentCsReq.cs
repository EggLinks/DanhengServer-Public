using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.PromoteEquipmentCsReq)]
public class HandlerPromoteEquipmentCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = PromoteEquipmentCsReq.Parser.ParseFrom(data);

        await connection.Player!.InventoryManager!.PromoteEquipment((int)req.EquipmentUniqueId);

        await connection.SendPacket(CmdIds.PromoteEquipmentScRsp);
    }
}