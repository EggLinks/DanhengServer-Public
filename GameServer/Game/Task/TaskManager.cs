using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Task.AvatarTask;

namespace EggLink.DanhengServer.GameServer.Game.Task;

public class TaskManager(PlayerInstance player) : BasePlayerManager(player)
{
    public PerformanceTrigger PerformanceTrigger { get; } = new(player);
    public LevelTask LevelTask { get; } = new(player);
    public AvatarLevelTask AvatarLevelTask { get; } = new();
    public MissionTaskTrigger MissionTaskTrigger { get; } = new(player);
    public SceneTaskTrigger SceneTaskTrigger { get; } = new(player);
}