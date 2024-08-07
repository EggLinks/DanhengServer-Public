using EggLink.DanhengServer.GameServer.Server.Packet.Send.Item;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Item;

[Opcode(CmdIds.ExpUpEquipmentCsReq)]
public class HandlerExpUpEquipmentCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = ExpUpEquipmentCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        var returnItem = await player.InventoryManager!.LevelUpEquipment((int)req.EquipmentUniqueId, req.CostData);

        await connection.SendPacket(new PacketExpUpEquipmentScRsp(returnItem));
    }
}