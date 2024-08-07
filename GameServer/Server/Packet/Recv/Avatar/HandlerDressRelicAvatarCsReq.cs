using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.DressRelicAvatarCsReq)]
public class HandlerDressRelicAvatarCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = DressRelicAvatarCsReq.Parser.ParseFrom(data);

        foreach (var param in req.SwitchList)
            await connection.Player!.InventoryManager!.EquipRelic((int)req.AvatarId, (int)param.RelicUniqueId,
                (int)param.RelicType);

        await connection.SendPacket(CmdIds.DressRelicAvatarScRsp);
    }
}