using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Game.Player;

namespace EggLink.DanhengServer.Game.Mission.FinishType
{
    public abstract class MissionFinishTypeHandler
    {
        public abstract void Init(PlayerInstance player, SubMissionInfo info, object? arg);

        public virtual void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }
    }
}
