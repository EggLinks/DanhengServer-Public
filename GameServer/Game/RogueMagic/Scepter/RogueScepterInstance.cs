using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.MagicUnit;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic.Scepter;

public class RogueScepterInstance(RogueMagicScepterExcel excel)
{
    public RogueMagicScepterExcel Excel { get; set; } = excel;
    public Dictionary<int, List<RogueMagicUnitInstance>> DressedUnits { get; set; } = [];

    public void AddUnit(int slot, RogueMagicUnitInstance unit)
    {
        DressedUnits.TryAdd(slot, []);

        DressedUnits[slot].Add(unit);
    }

    public void RemoveUnit(int slot)
    {
        DressedUnits.Remove(slot);
    }

    public RogueMagicGameScepterInfo ToProto()
    {
        var proto = new RogueMagicGameScepterInfo
        {
            ModifierContent = ToBasicInfo()
        };

        foreach (var dressedUnit in DressedUnits)
        foreach (var unit in dressedUnit.Value)
            proto.ScepterDressInfo.Add(new RogueMagicScepterDressInfo
            {
                Slot = (uint)dressedUnit.Key,
                DressMagicUnitUniqueId = (uint)unit.UniqueId,
                Type = (uint)unit.Excel.MagicUnitType
            });

        foreach (var trench in Excel.TrenchCount) proto.TrenchCount.Add((uint)trench.Key, (uint)trench.Value);

        foreach (var unitInfo in Excel.LockMagicUnit)
            proto.LockedMagicUnitList.Add(new RogueMagicGameUnit
            {
                MagicUnitId = (uint)unitInfo.MagicUnitId,
                Level = (uint)unitInfo.MagicUnitLevel
            });

        return proto;
    }

    public RogueCommonActionResult ToGetInfo(RogueCommonActionResultSourceType source)
    {
        return new RogueCommonActionResult
        {
            Source = source,
            RogueAction = new RogueCommonActionResultData
            {
                GetScepterList = ToGetInfo()
            }
        };
    }

    public RogueCommonActionResult ToDressInfo(RogueCommonActionResultSourceType source)
    {
        return new RogueCommonActionResult
        {
            Source = source,
            RogueAction = new RogueCommonActionResultData
            {
                DressScepterList = ToDressInfo()
            }
        };
    }

    public BattleRogueMagicScepter ToBattleScepterInfo()
    {
        var proto = new BattleRogueMagicScepter
        {
            ScepterId = (uint)Excel.ScepterID,
            Level = (uint)Excel.ScepterLevel
        };

        foreach (var trench in Excel.TrenchCount) proto.TrenchCount.Add((uint)trench.Key, (uint)trench.Value);

        foreach (var unitInfo in Excel.LockMagicUnit)
            proto.RogueMagicUnitInfoList.Add(new BattleRogueMagicUnit
            {
                MagicUnitId = (uint)unitInfo.MagicUnitId,
                Level = (uint)unitInfo.MagicUnitLevel
            });

        foreach (var unitInfo in DressedUnits)
        foreach (var unit in unitInfo.Value)
            proto.RogueMagicUnitInfoList.Add(new BattleRogueMagicUnit
            {
                MagicUnitId = (uint)unit.Excel.MagicUnitID,
                Level = (uint)unit.Excel.MagicUnitLevel,
                DiceSlotId = (uint)unitInfo.Key
            });

        return proto;
    }

    public RogueMagicScepter ToBasicInfo()
    {
        var proto = new RogueMagicScepter
        {
            ScepterId = (uint)Excel.ScepterID,
            Level = (uint)Excel.ScepterLevel
        };

        return proto;
    }

    public RogueCommonGetScepter ToGetInfo()
    {
        var proto = new RogueCommonGetScepter
        {
            UpdateScepterInfo = ToProto()
        };

        return proto;
    }

    public RogueCommonDressScepter ToDressInfo()
    {
        var proto = new RogueCommonDressScepter
        {
            UpdateScepterInfo = ToProto()
        };

        return proto;
    }
}