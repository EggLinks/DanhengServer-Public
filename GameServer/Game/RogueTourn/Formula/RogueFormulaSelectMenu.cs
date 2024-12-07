using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Rogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueTourn.Formula;

public class RogueFormulaSelectMenu(BaseRogueInstance rogue)
{
    public List<RogueTournFormulaExcel> Formulas { get; set; } = [];
    public int RollMaxCount { get; set; } = rogue.BaseRerollCount;
    public int RollCount { get; set; }
    public int RollFreeCount { get; set; } = rogue.BaseRerollFreeCount;
    public int RollCost { get; set; } = rogue.CurRerollCost;
    public int QueueAppend { get; set; } = 3;
    public List<RogueTournFormulaExcel> FormulaPool { get; set; } = [];

    public void RollFormula(List<RogueTournFormulaExcel> formulas, int count = 3)
    {
        FormulaPool.Clear();
        FormulaPool.AddRange(formulas);

        var list = new RandomList<RogueTournFormulaExcel>();

        foreach (var formula in formulas)
            list.Add(formula, (int)(5 - formula.FormulaCategory));
        var result = new List<RogueTournFormulaExcel>();

        for (var i = 0; i < count; i++)
        {
            var formulaExcel = list.GetRandom();
            if (formulaExcel != null)
            {
                result.Add(formulaExcel);
                list.Remove(formulaExcel);
            }

            if (list.GetCount() == 0) break; // No more formulas to roll
        }

        Formulas = result;
    }

    public async ValueTask RerollFormula()
    {
        if (RollFreeCount > 0)
        {
            RollFreeCount--; // Free reroll
        }
        else
        {
            if (RollMaxCount - RollCount <= 0) return;
            RollCount++; // Paid reroll
            await rogue.CostMoney(RollCost);
        }

        RollFormula(FormulaPool.ToList());
    }

    public RogueActionInstance GetActionInstance()
    {
        rogue.CurActionQueuePosition += QueueAppend;
        return new RogueActionInstance
        {
            QueuePosition = rogue.CurActionQueuePosition,
            RogueFormulaSelectMenu = this
        };
    }


    public RogueFormulaSelectInfo ToProto()
    {
        return new RogueFormulaSelectInfo
        {
            CanRoll = false,
            RollFormulaCount = (uint)RollCount,
            RollFormulaFreeCount = (uint)RollFreeCount,
            RollFormulaMaxCount = (uint)RollMaxCount,
            RollFormulaCostData = new ItemCostData
            {
                ItemList =
                {
                    new ItemCost
                    {
                        PileItem = new PileItem
                        {
                            ItemId = 31,
                            ItemNum = (uint)RollCost
                        }
                    }
                }
            },
            SelectFormulaIdList = { Formulas.Select(x => (uint)x.FormulaID) },
            HandbookUnlockFormulaIdList = { Formulas.Select(x => (uint)x.FormulaID) }
        };
    }
}