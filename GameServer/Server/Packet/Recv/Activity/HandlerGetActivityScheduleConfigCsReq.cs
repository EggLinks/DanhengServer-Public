using EggLink.DanhengServer.GameServer.Server.Packet.Send.Activity;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Activity;

[Opcode(CmdIds.GetActivityScheduleConfigCsReq)]
public class HandlerGetActivityScheduleConfigCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetActivityScheduleConfigScRsp(connection.Player!));
    }
}