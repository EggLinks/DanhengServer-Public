using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.MapRotation;

[Opcode(CmdIds.LeaveMapRotationRegionCsReq)]
public class HandlerLeaveMapRotationRegionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(CmdIds.LeaveMapRotationRegionScRsp);
    }
}