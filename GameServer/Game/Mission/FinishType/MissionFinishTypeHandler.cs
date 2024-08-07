using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType;

public abstract class MissionFinishTypeHandler
{
    public abstract ValueTask HandleMissionFinishType(PlayerInstance player, SubMissionInfo info, object? arg);

    public abstract ValueTask HandleQuestFinishType(PlayerInstance player, QuestDataExcel quest, FinishWayExcel excel,
        object? arg);
}