using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction.Handler;

[MissionFinishAction(FinishActionTypeEnum.MoveToAnchor)]
public class MissionHandlerMoveToAnchor : MissionFinishActionHandler
{
    public override async ValueTask OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
    {
        var entryId = Params[0];
        var anchorGroup = Params[1];
        var anchorId = Params[2];
        await Player.EnterMissionScene(entryId, anchorGroup, anchorId, true);
    }
}