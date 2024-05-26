using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Mission.FinishAction.Handler
{
    [MissionFinishAction(FinishActionTypeEnum.addMissionItem)]
    public class MissionHandlerAddMissionItem : MissionFinishActionHandler
    {
        public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
        {
            if (Params.Count < 2) return;
            var itemId = Params[0];
            var count = Params[1];
            Player.InventoryManager!.AddItem(itemId, count);
        }
    }
}
