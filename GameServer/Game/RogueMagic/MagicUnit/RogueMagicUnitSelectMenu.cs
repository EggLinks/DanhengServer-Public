using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Rogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic.MagicUnit;

public class RogueMagicUnitSelectMenu(BaseRogueInstance rogue) : BaseRogueSelectMenu
{
    public List<RogueMagicUnitExcel> MagicUnits { get; set; } = [];
    public int RollMaxCount { get; set; } = rogue.BaseRerollCount;
    public int RollCount { get; set; }
    public int RollFreeCount { get; set; } = rogue.BaseRerollFreeCount;
    public int RollCost { get; set; } = rogue.CurRerollCost;
    public int Count { get; set; } = 3;
    public int QueueAppend { get; set; } = 2;
    public List<RogueMagicUnitExcel> MagicUnitPool { get; set; } = [];

    public override void Roll()
    {
        if (MagicUnits.Count > 0) return; // already init
        // Remove existing magic units
        if (rogue is RogueMagicInstance magic)
            foreach (var excel in MagicUnitPool.Clone())
                if (magic.RogueMagicUnits.Any(x => x.Value.Excel.MagicUnitID == excel.MagicUnitID))
                    MagicUnitPool.Remove(excel);
        var list = new RandomList<RogueMagicUnitExcel>();

        foreach (var unitExcel in MagicUnitPool)
            list.Add(unitExcel, 1);
        var result = new List<RogueMagicUnitExcel>();

        for (var i = 0; i < Count; i++)
        {
            var unitExcel = list.GetRandom();
            if (unitExcel != null)
            {
                result.Add(unitExcel);
                list.Remove(unitExcel);
            }

            if (list.GetCount() == 0) break; // No more magic unit to roll
        }

        MagicUnits = result;
    }

    public void SetPool(List<RogueMagicUnitExcel> magicUnits, int count = 3)
    {
        MagicUnitPool.Clear();
        MagicUnitPool.AddRange(magicUnits);
        Count = count;
    }

    public async ValueTask RerollMagicUnit()
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

        Roll();
    }

    public RogueActionInstance GetActionInstance()
    {
        rogue.CurActionQueuePosition += QueueAppend;
        return new RogueActionInstance
        {
            QueuePosition = rogue.CurActionQueuePosition,
            RogueMagicUnitSelectMenu = this
        };
    }


    public RogueMagicUnitSelectInfo ToProto()
    {
        return new RogueMagicUnitSelectInfo
        {
            SelectMagicUnits =
            {
                MagicUnits.Select(x => new RogueMagicGameUnit
                {
                    MagicUnitId = (uint)x.MagicUnitID,
                    Level = (uint)x.MagicUnitLevel
                })
            },
            SelectHintId = 260002,
            ABHPIGOGACI = 1,
            OMPAAKLLLFD = 1
        };
    }
}