using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Game.Mission.FinishType;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(Enums.MissionFinishTypeEnum.GetItem)]
    internal class MissionHandlerGetItem : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (arg != null && arg is ItemData item)
            {
                if (item.ItemId == info.ParamInt1)
                {
                    player.MissionManager!.FinishSubMission(info.ID);
                }
            }
        }
    }
}
