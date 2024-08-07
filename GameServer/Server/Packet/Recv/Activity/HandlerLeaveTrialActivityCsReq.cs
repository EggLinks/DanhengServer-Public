using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Activity;

[Opcode(CmdIds.LeaveTrialActivityCsReq)]
public class HandlerLeaveTrialActivityCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = LeaveTrialActivityCsReq.Parser.ParseFrom(data);

        if (connection.Player!.ActivityManager!.TrialActivityInstance != null)
        {
            var manager = connection.Player!.ActivityManager;
            await manager.TrialActivityInstance.EndActivity();
        }

        connection.Player!.ActivityManager!.TrialActivityInstance = null;

        await connection.SendPacket(CmdIds.LeaveTrialActivityScRsp);
    }
}