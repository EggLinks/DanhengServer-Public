using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.BattleChallenge)]
public class MissionHandlerBattleChallenge : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        // this type wont be used in mission
        await ValueTask.CompletedTask;
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        if (arg is BattleInstance instance)
        {
            var progress = 0;
            if (instance.BattleResult == null) return;
            foreach (var battleTargetList in instance.BattleResult.Stt.BattleTargetInfo.Values)
            foreach (var battleTarget in battleTargetList.BattleTargetList_)
                if (excel.ParamIntList.Contains((int)battleTarget.Id))
                    progress += (int)battleTarget.Progress;

            await player.QuestManager!.UpdateQuestProgress(quest.QuestID, progress);
        }
    }
}