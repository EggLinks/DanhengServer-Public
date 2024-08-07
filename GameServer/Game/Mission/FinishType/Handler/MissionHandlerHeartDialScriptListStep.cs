using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.HeartDialScriptListStep)]
public class MissionHandlerHeartDialScriptListStep : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        var count = 0;
        foreach (var scriptId in info.ParamIntList ?? [])
        {
            HeartDialInfo? dialInfo = null;
            player.HeartDialData?.DialList.TryGetValue(scriptId, out dialInfo);
            if (dialInfo != null && (int)dialInfo.StepType == info.ParamInt1) count++;
        }

        if (count >= info.Progress)
        {
            await player.MissionManager!.FinishSubMission(info.ID);
        }
        else
        {
            if (player.MissionManager?.GetMissionProgress(info.ID) != count)
                await player.MissionManager!.SetMissionProgress(info.ID, count);
        }
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        // this type wont be used in quest
        await ValueTask.CompletedTask;
    }
}