using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.RaidFinishedAndTransfer)]
public class MissionHandlerRaidFinishedAndTransfer : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        if (player.RaidManager!.GetRaidStatus(info.ParamInt1) == RaidStatus.Finish)
            await player.MissionManager!.FinishSubMission(info.ID);
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        await ValueTask.CompletedTask;
    }
}