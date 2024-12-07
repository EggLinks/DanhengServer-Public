using EggLink.DanhengServer.GameServer.Server.Packet.Send.Pet;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Pet;

[Opcode(CmdIds.GetPetDataCsReq)]
public class HandlerGetPetDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;

        await connection.SendPacket(new PacketGetPetDataScRsp(player));
    }
}