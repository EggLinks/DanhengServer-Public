using EggLink.DanhengServer.GameServer.Game.Rogue.Buff;
using EggLink.DanhengServer.GameServer.Game.Rogue.Miracle;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.MagicUnit;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.Scepter;
using EggLink.DanhengServer.GameServer.Game.RogueTourn.Formula;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue;

public class RogueActionInstance
{
    public int QueuePosition { get; set; } = 0;
    public RogueBuffSelectMenu? RogueBuffSelectMenu { get; set; }
    public RogueMiracleSelectMenu? RogueMiracleSelectMenu { get; set; }
    public RogueBonusSelectInfo? RogueBonusSelectInfo { get; set; }
    public RogueFormulaSelectMenu? RogueFormulaSelectMenu { get; set; }
    public RogueMagicUnitSelectMenu? RogueMagicUnitSelectMenu { get; set; }
    public RogueScepterSelectMenu? RogueScepterSelectMenu { get; set; }

    public bool IsReforge { get; set; }

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

        if (RogueBuffSelectMenu != null && !IsReforge) action.BuffSelectInfo = RogueBuffSelectMenu.ToProto();

        if (RogueBuffSelectMenu != null && IsReforge)
            action.BuffReforgeSelectInfo = RogueBuffSelectMenu.ToReforgeProto();

        if (RogueMiracleSelectMenu != null) action.MiracleSelectInfo = RogueMiracleSelectMenu.ToProto();

        if (RogueBonusSelectInfo != null) action.BonusSelectInfo = RogueBonusSelectInfo;

        if (RogueFormulaSelectMenu != null) action.RogueFormulaSelectInfo = RogueFormulaSelectMenu.ToProto();

        if (RogueMagicUnitSelectMenu != null) action.MagicUnitSelectInfo = RogueMagicUnitSelectMenu.ToProto();

        if (RogueScepterSelectMenu != null) action.ScepterSelectInfo = RogueScepterSelectMenu.ToProto();

        return new RogueCommonPendingAction
        {
            QueuePosition = (uint)QueuePosition,
            RogueAction = action
        };
    }

    public BaseRogueSelectMenu? GetSelectMenu()
    {
        //if (RogueBuffSelectMenu != null) return RogueBuffSelectMenu;
        //if (RogueMiracleSelectMenu != null) return RogueMiracleSelectMenu;
        //if (RogueFormulaSelectMenu != null) return RogueFormulaSelectMenu;
        if (RogueMagicUnitSelectMenu != null) return RogueMagicUnitSelectMenu;
        if (RogueScepterSelectMenu != null) return RogueScepterSelectMenu;

        return null;
    }
}