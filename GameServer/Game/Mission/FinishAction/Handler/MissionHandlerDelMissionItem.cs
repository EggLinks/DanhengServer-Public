using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Mission.FinishAction;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction.Handler
{
    [MissionFinishAction(FinishActionTypeEnum.delMissionItem)]
    public class MissionHandlerDelMissionItem : MissionFinishActionHandler
    {
        public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
        {
            if (Params.Count < 2) return;
            for (var i = 0; i < Params.Count; i += 2)
            {
                var itemId = Params[i];
                var count = Params[i + 1];
                Player.InventoryManager!.RemoveItem(itemId, count);
            }
        }
    }
}
