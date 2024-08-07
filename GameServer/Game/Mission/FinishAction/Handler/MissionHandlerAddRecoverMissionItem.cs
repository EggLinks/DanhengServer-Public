using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction.Handler;

[MissionFinishAction(FinishActionTypeEnum.addRecoverMissionItem)]
public class MissionHandlerAddRecoverMissionItem : MissionFinishActionHandler
{
    public override async ValueTask OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
    {
        if (Params.Count < 2) return;

        for (var i = 0; i < Params.Count; i += 2)
        {
            var itemId = Params[i];
            var count = Params[i + 1];
            await Player.InventoryManager!.AddItem(itemId, count);
        }
    }
}