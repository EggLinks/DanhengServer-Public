using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.EnterMapByEntrance)]
public class MissionHandlerEnterMapByEntrance : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        if (arg is MapEntranceExcel v)
            if (v.ID == info.ParamInt1)
                await player.MissionManager!.FinishSubMission(info.ID);
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        // this type wont be used in quest
        if (arg is MapEntranceExcel v)
            if (v.ID == excel.ParamInt1)
                await player.QuestManager!.AddQuestProgress(quest.QuestID, 1);
    }
}