using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Game.Mission.FinishType;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(Enums.MissionFinishTypeEnum.ItemNum)]
    public class MissionHandlerItemNum : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            var count = 0;
            foreach (var itemId in info.ParamIntList ?? [])
            {
                var item = player.InventoryManager?.GetItem(itemId);
                if (item != null)
                {
                    count += item.Count;
                }
            }

            if (count == info.Progress)
            {
                player.MissionManager?.FinishSubMission(info.ID);
            }
            else
            {
                if (player.MissionManager?.GetMissionProgress(info.ID) != count)
                    player.MissionManager?.SetMissionProgress(info.ID, count);
            }
        }
    }
}
