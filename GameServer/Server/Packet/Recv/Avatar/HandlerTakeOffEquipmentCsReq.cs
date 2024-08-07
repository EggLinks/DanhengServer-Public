using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.TakeOffEquipmentCsReq)]
public class HandlerTakeOffEquipmentCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = TakeOffEquipmentCsReq.Parser.ParseFrom(data);
        await connection.Player!.InventoryManager!.UnequipEquipment((int)req.AvatarId);

        await connection.SendPacket(CmdIds.TakeOffEquipmentScRsp);
    }
}