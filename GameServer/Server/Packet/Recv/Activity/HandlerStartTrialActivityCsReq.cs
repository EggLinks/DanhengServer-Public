using EggLink.DanhengServer.GameServer.Game.Activity.Activities;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Activity;

[Opcode(CmdIds.StartTrialActivityCsReq)]
public class HandlerStartTrialActivityCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = StartTrialActivityCsReq.Parser.ParseFrom(data);

        var activityManager = connection.Player!.ActivityManager!;
        activityManager.TrialActivityInstance = new TrialActivityInstance(activityManager);
        await activityManager.TrialActivityInstance.StartActivity((int)req.StageId);
    }
}