using EggLink.DanhengServer.GameServer.Game.Rogue.Buff;
using EggLink.DanhengServer.GameServer.Game.Rogue.Miracle;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue;

public class RogueActionInstance
{
    public int QueuePosition { get; set; } = 0;
    public RogueBuffSelectMenu? RogueBuffSelectMenu { get; set; }
    public RogueMiracleSelectMenu? RogueMiracleSelectMenu { get; set; }
    public RogueBonusSelectInfo? RogueBonusSelectInfo { get; set; }

    public void SetBonus()
    {
        RogueBonusSelectInfo = new RogueBonusSelectInfo
        {
            BonusIdList = { 4, 5, 6 }
        };
    }

    public RogueCommonPendingAction ToProto()
    {
        var action = new RogueAction();

        if (RogueBuffSelectMenu != null) action.BuffSelectInfo = RogueBuffSelectMenu.ToProto();

        if (RogueMiracleSelectMenu != null) action.MiracleSelectInfo = RogueMiracleSelectMenu.ToProto();

        if (RogueBonusSelectInfo != null) action.BonusSelectInfo = RogueBonusSelectInfo;

        return new RogueCommonPendingAction
        {
            QueuePosition = (uint)QueuePosition,
            RogueAction = action
        };
    }
}