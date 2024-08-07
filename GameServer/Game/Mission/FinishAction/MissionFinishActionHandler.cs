using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction;

public abstract class MissionFinishActionHandler
{
    public abstract ValueTask OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player);
}