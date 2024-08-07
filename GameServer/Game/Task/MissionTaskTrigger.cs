using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Task;

public class MissionTaskTrigger(PlayerInstance player)
{
    public PlayerInstance Player { get; } = player;

    public void TriggerMissionTask(int missionId)
    {
        GameData.SubMissionData.TryGetValue(missionId, out var subMission);
        if (subMission != null)
            TriggerMissionTask(subMission.SubMissionTaskInfo ?? new LevelGraphConfigInfo(), subMission);
    }

    public void TriggerMissionTask(LevelGraphConfigInfo subMissionTaskInfo, SubMissionExcel subMission)
    {
        foreach (var task in subMissionTaskInfo.OnInitSequece)
            Player.TaskManager?.LevelTask.TriggerInitAct(task, subMission);

        foreach (var task in subMissionTaskInfo.OnStartSequece)
            Player.TaskManager?.LevelTask.TriggerStartAct(task, subMission);
    }
}