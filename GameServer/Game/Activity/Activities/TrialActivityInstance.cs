using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Activity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Activity;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Activity.Activities;

public class TrialActivityInstance : BaseActivityInstance
{
    public TrialActivityInstance(ActivityManager manager) : base(manager)
    {
        Data = ActivityManager.Data.TrialActivityData;
    }

    public TrialActivityData Data { get; set; }

    public async ValueTask StartActivity(int stageId)
    {
        var player = ActivityManager.Player;

        await player.LineupManager!.DestroyExtraLineup(ExtraLineupType.LineupStageTrial);

        GameData.AvatarDemoConfigData.TryGetValue(stageId, out var excel);
        if (excel != null)
        {
            Data.CurTrialStageId = stageId;
            player.LineupManager.SetExtraLineup(ExtraLineupType.LineupStageTrial, excel.TrialAvatarList.ToList());
            await player.EnterScene(excel.MapEntranceID, 0, true);
        }

        await player.SendPacket(new PacketStartTrialActivityScRsp((uint)stageId));
    }

    public async ValueTask EndActivity(TrialActivityStatus status = TrialActivityStatus.None)
    {
        var player = ActivityManager.Player!;

        // Remove trial lineup
        await player.LineupManager!.DestroyExtraLineup(ExtraLineupType.LineupStageTrial);
        player.LineupManager!.LineupData.CurExtraLineup = -1;

        // Go back to default scene
        await player.EnterScene(2000101, 0, true);
        if (status == TrialActivityStatus.Finish)
        {
            Data.Activities.Add(new TrialActivityResultData
            {
                StageId = Data.CurTrialStageId
            });
            await player.SendPacket(new PacketCurTrialActivityScNotify((uint)Data.CurTrialStageId, status));
        }

        Data.CurTrialStageId = 0;
    }
}