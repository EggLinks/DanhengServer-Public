using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.FinishQuest)]
public class MissionHandlerFinishQuest : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        // this type wont be used in mission
        await ValueTask.CompletedTask;
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        var questCount = 0;
        foreach (var qid in excel.ParamIntList)
        {
            var status = player.QuestManager?.GetQuestStatus(qid);
            if (status == QuestStatus.QuestFinish || status == QuestStatus.QuestClose)
                questCount++;
        }

        await player.QuestManager!.UpdateQuestProgress(quest.QuestID, questCount);
    }
}