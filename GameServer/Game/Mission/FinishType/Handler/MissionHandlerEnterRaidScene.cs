using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler;

[MissionFinishType(MissionFinishTypeEnum.EnterRaidScene)]
public class MissionHandlerEnterRaidScene : MissionFinishTypeHandler
{
    public override async ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
    {
        if (player.RaidManager!.RaidData.CurRaidId != info.ParamInt1)
            // change raid
            await player.RaidManager!.EnterRaid(info.ParamInt1, 0);
        await player.EnterScene(info.ParamInt2, 0, true);
        await player.MissionManager!.FinishSubMission(info.ID);
    }

    public override async ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest,
        FinishWayExcel excel, object? arg)
    {
        // this type wont be used in quest
        await ValueTask.CompletedTask;
    }
}