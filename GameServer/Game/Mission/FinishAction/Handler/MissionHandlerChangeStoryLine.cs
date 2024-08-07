using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction.Handler;

[MissionFinishAction(FinishActionTypeEnum.ChangeStoryLine)]
public class MissionHandlerChangeStoryLine : MissionFinishActionHandler
{
    public override async ValueTask OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
    {
        var toStoryLineId = Params[0];
        var toEntryId = Params[1];
        var toAnchorGroup = Params[2];
        var toAnchorId = Params[3];

        if (toStoryLineId == 0)
            // exit
            await Player.StoryLineManager!.FinishStoryLine(toEntryId, toAnchorGroup, toAnchorId);
        else
            await Player.StoryLineManager!.InitStoryLine(toStoryLineId, toEntryId, toAnchorGroup, toAnchorId);
    }
}