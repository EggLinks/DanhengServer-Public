using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Rogue.Buff;
using EggLink.DanhengServer.Game.Rogue.Miracle;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue
{
    public class RogueActionInstance
    {
        public int QueuePosition { get; set; } = 0;
        public RogueBuffSelectMenu? RogueBuffSelectMenu { get; set; }
        public RogueMiracleSelectMenu? RogueMiracleSelectMenu { get; set; }
        public RogueBonusSelectInfo? RogueBonusSelectInfo { get; set; }

        public void SetBonus()
        {
            RogueBonusSelectInfo = new()
            {
                BonusIdList = { 4, 5, 6 }
            };
        }

        public RogueCommonPendingAction ToProto()
        {
            var action = new RogueAction();

            if (RogueBuffSelectMenu != null)
            {
                action.BuffSelectInfo = RogueBuffSelectMenu.ToProto();
            }

            if (RogueMiracleSelectMenu != null)
            {
                action.MiracleSelectInfo = RogueMiracleSelectMenu.ToProto();
            }

            if (RogueBonusSelectInfo != null)
            {
                action.BonusSelectInfo = RogueBonusSelectInfo;
            }

            return new()
            {
                QueuePosition = (uint)QueuePosition,
                RogueAction = action
            };
        }
    }
}
