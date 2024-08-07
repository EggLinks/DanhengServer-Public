using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Challenge;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.ChallengeFinish)]
public class MissionHandlerChallengeFinish : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        if (arg is ChallengeInstance challenge)
            if (challenge.Excel.ID == info.ParamInt1 && challenge.IsWin())
                await player.MissionManager!.FinishSubMission(info.ID);
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        if (arg is ChallengeInstance challenge)
            if (challenge.Excel.ID == excel.ParamInt1 && challenge.IsWin())
                await player.QuestManager!.AddQuestProgress(quest.QuestID, 1);
    }
}