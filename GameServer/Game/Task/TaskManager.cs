using EggLink.DanhengServer.Game;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Task
{
    public class TaskManager(PlayerInstance player) : BasePlayerManager(player)
    {
        public PerformanceTrigger PerformanceTrigger { get; } = new(player);
        public LevelTask LevelTask { get; } = new(player);
        public MissionTaskTrigger MissionTaskTrigger { get; } = new(player);
        public SceneTaskTrigger SceneTaskTrigger { get; } = new(player);
    }
}
