using EggLink.DanhengServer.GameServer.Server.Packet.Send.Phone;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Phone;

[Opcode(CmdIds.GetPhoneDataCsReq)]
public class HandlerGetPhoneDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetPhoneDataScRsp(connection.Player!));
    }
}