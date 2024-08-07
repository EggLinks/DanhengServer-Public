using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Task;

public class TaskManager(PlayerInstance player) : BasePlayerManager(player)
{
    public PerformanceTrigger PerformanceTrigger { get; } = new(player);
    public LevelTask LevelTask { get; } = new(player);
    public MissionTaskTrigger MissionTaskTrigger { get; } = new(player);
    public SceneTaskTrigger SceneTaskTrigger { get; } = new(player);
}