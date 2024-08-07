using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Task;

public class PerformanceTrigger(PlayerInstance player)
{
    public PlayerInstance Player { get; } = player;

    public void TriggerPerformanceE(int performanceEId, SubMissionExcel subMission)
    {
        GameData.PerformanceEData.TryGetValue(performanceEId, out var excel);
        if (excel != null) TriggerPerformanceE(excel, subMission);
    }

    public void TriggerPerformanceE(PerformanceEExcel excel, SubMissionExcel subMission)
    {
        if (excel.ActInfo == null) return;
        foreach (var act in excel.ActInfo.OnInitSequece) Player.TaskManager?.LevelTask.TriggerInitAct(act, subMission);

        foreach (var act in excel.ActInfo.OnStartSequece)
            Player.TaskManager?.LevelTask.TriggerStartAct(act, subMission);
    }

    public void TriggerPerformanceD(int performanceDId, SubMissionExcel subMission)
    {
        GameData.PerformanceDData.TryGetValue(performanceDId, out var excel);
        if (excel != null) TriggerPerformanceD(excel, subMission);
    }

    public void TriggerPerformanceD(PerformanceDExcel excel, SubMissionExcel subMission)
    {
        if (excel.ActInfo == null) return;
        foreach (var act in excel.ActInfo.OnInitSequece) Player.TaskManager?.LevelTask.TriggerInitAct(act, subMission);

        foreach (var act in excel.ActInfo.OnStartSequece)
            Player.TaskManager?.LevelTask.TriggerStartAct(act, subMission);
    }
}