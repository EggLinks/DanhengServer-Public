using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Mission.FinishType;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.UseItem)]
    public class MissionHandlerUseItem : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (arg is ItemData item)
            {
                if (info.ParamInt1 == item.ItemId)
                {
                    player.MissionManager?.AddMissionProgress(info.ID, 1);
                }
            }

            if (player.MissionManager?.GetMissionProgress(info.ID) >= info.Progress)
            {
                player.MissionManager?.FinishSubMission(info.ID);
            }
        }
    }
}
