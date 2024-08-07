using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction.Handler;

[MissionFinishAction(FinishActionTypeEnum.delSubMission)]
public class MissionHandlerDelSubMission : MissionFinishActionHandler
{
    public override async ValueTask OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
    {
        if (Params.Count < 1) return;

        foreach (var subMissionId in Params)
        {
            await Player.MissionManager!.AcceptSubMission(subMissionId);
            await Player.MissionManager!.FinishSubMission(subMissionId);
        }
    }
}