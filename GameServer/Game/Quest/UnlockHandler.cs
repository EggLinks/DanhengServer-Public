using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Enums.Quest;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Quest;

public class UnlockHandler(PlayerInstance player)
{
    public PlayerInstance Player { get; } = player;

    public bool GetUnlockStatus(int unlockId)
    {
        GameData.FuncUnlockDataData.TryGetValue(unlockId, out var unlockData);
        if (unlockData == null) return false;

        // judge
        foreach (var condition in unlockData.Conditions)
            switch (condition.Type)
            {
                case ConditionTypeEnum.WorldLevel:
                    if (Player.Data.WorldLevel < int.Parse(condition.Param)) return false; // less than it
                    break;
                case ConditionTypeEnum.FinishMainMission:
                    if (Player.MissionManager?.GetMainMissionStatus(int.Parse(condition.Param)) !=
                        MissionPhaseEnum.Finish) return false;
                    break;
                case ConditionTypeEnum.InStoryLine:
                    if (Player.StoryLineManager?.StoryLineData.CurStoryLineId != int.Parse(condition.Param))
                        return false;
                    break;
                case ConditionTypeEnum.PlayerLevel:
                    if (Player.Data.Level < int.Parse(condition.Param)) return false;
                    break;
                case ConditionTypeEnum.FinishSubMission:
                    if (Player.MissionManager?.GetSubMissionStatus(int.Parse(condition.Param)) !=
                        MissionPhaseEnum.Finish) return false;
                    break;
            }

        return true;
    }
}