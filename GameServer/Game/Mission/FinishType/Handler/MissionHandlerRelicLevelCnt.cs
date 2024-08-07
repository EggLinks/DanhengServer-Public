using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.RelicLevelCnt)]
public class MissionHandlerRelicLevelCnt : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        // this type wont be used in mission
        await ValueTask.CompletedTask;
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        var relicCount = 0;
        foreach (var relic in player.InventoryManager?.Data.RelicItems ?? [])
            if (relic.Level >= excel.ParamInt1)
                relicCount++;

        await player.QuestManager!.UpdateQuestProgress(quest.QuestID, relicCount);
    }
}