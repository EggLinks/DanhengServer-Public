using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Inventory;

[SugarTable("InventoryData")]
public class InventoryData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public List<ItemData> MaterialItems { get; set; } = [];

    [SugarColumn(IsJson = true)] public List<ItemData> EquipmentItems { get; set; } = [];

    [SugarColumn(IsJson = true)] public List<ItemData> RelicItems { get; set; } = [];

    public int NextUniqueId { get; set; } = 100;
}

public class ItemData
{
    public int UniqueId { get; set; }
    public int ItemId { get; set; }
    public int Count { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int TotalExp { get; set; }
    public int Promotion { get; set; }
    public int Rank { get; set; } // Superimpose
    public bool Locked { get; set; }
    public bool Discarded { get; set; }

    public int MainAffix { get; set; }
    public List<ItemSubAffix> SubAffixes { get; set; } = [];

    public int EquipAvatar { get; set; }

    #region Action

    public void AddRandomRelicMainAffix()
    {
        GameData.RelicConfigData.TryGetValue(ItemId, out var config);
        if (config == null) return;
        var affixId = UtilTools.GetRandomRelicMainAffix(config.MainAffixGroup);
        MainAffix = affixId;
    }

    public void IncreaseRandomRelicSubAffix()
    {
        GameData.RelicConfigData.TryGetValue(ItemId, out var config);
        if (config == null) return;
        GameData.RelicSubAffixData.TryGetValue(config.SubAffixGroup, out var affixes);
        if (affixes == null) return;
        var element = SubAffixes.RandomElement();
        var affix = affixes.Values.ToList().Find(x => x.AffixID == element.Id);
        if (affix == null) return;
        element.IncreaseStep(affix.StepNum);
    }

    public void AddRandomRelicSubAffix(int count = 1)
    {
        GameData.RelicConfigData.TryGetValue(ItemId, out var config);
        if (config == null) return;
        GameData.RelicSubAffixData.TryGetValue(config.SubAffixGroup, out var affixes);

        if (affixes == null) return;

        var rollPool = new List<RelicSubAffixConfigExcel>();
        foreach (var affix in affixes.Values)
            if (SubAffixes.Find(x => x.Id == affix.AffixID) == null)
                rollPool.Add(affix);

        for (var i = 0; i < count; i++)
        {
            var affixConfig = rollPool.RandomElement();
            ItemSubAffix subAffix = new(affixConfig, 1);
            SubAffixes.Add(subAffix);
            rollPool.Remove(affixConfig);
        }
    }

    #endregion

    #region Serialization

    public Material ToMaterialProto()
    {
        return new Material
        {
            Tid = (uint)ItemId,
            Num = (uint)Count
        };
    }

    public Relic ToRelicProto()
    {
        Relic relic = new()
        {
            Tid = (uint)ItemId,
            UniqueId = (uint)UniqueId,
            Level = (uint)Level,
            IsProtected = Locked,
            Exp = (uint)Exp,
            IsDiscarded = Discarded,
            DressAvatarId = (uint)EquipAvatar,
            MainAffixId = (uint)MainAffix
        };
        if (SubAffixes.Count >= 1)
            foreach (var subAffix in SubAffixes)
                relic.SubAffixList.Add(subAffix.ToProto());
        return relic;
    }

    public Equipment ToEquipmentProto()
    {
        return new Equipment
        {
            Tid = (uint)ItemId,
            UniqueId = (uint)UniqueId,
            Level = (uint)Level,
            Exp = (uint)Exp,
            IsProtected = Locked,
            Promotion = (uint)Promotion,
            Rank = (uint)Rank,
            DressAvatarId = (uint)EquipAvatar
        };
    }

    public ChallengeBossEquipmentInfo ToChallengeEquipmentProto()
    {
        return new ChallengeBossEquipmentInfo
        {
            Tid = (uint)ItemId,
            UniqueId = (uint)UniqueId,
            Level = (uint)Level,
            Promotion = (uint)Promotion,
            Rank = (uint)Rank
        };
    }

    public ChallengeBossRelicInfo ToChallengeRelicProto()
    {
        var proto = new ChallengeBossRelicInfo
        {
            Tid = (uint)ItemId,
            UniqueId = (uint)UniqueId,
            Level = (uint)Level,
            MainAffixId = (uint)MainAffix
        };

        if (SubAffixes.Count < 1) return proto;
        foreach (var subAffix in SubAffixes)
            proto.SubAffixList.Add(subAffix.ToProto());

        return proto;
    }

    public Item ToProto()
    {
        return new Item
        {
            ItemId = (uint)ItemId,
            Num = (uint)Count,
            Level = (uint)Level,
            MainAffixId = (uint)MainAffix,
            Rank = (uint)Rank,
            Promotion = (uint)Promotion,
            UniqueId = (uint)UniqueId
        };
    }

    public PileItem ToPileProto()
    {
        return new PileItem
        {
            ItemId = (uint)ItemId,
            ItemNum = (uint)Count
        };
    }

    public DisplayEquipmentInfo ToDisplayEquipmentProto()
    {
        return new DisplayEquipmentInfo
        {
            Tid = (uint)ItemId,
            Level = (uint)Level,
            Exp = (uint)Exp,
            Promotion = (uint)Promotion,
            Rank = (uint)Rank
        };
    }

    public DisplayRelicInfo ToDisplayRelicProto()
    {
        DisplayRelicInfo relic = new()
        {
            Tid = (uint)ItemId,
            Level = (uint)Level,
            Type = (uint)GameData.RelicConfigData[ItemId].Type,
            MainAffixId = (uint)MainAffix
        };

        if (SubAffixes.Count >= 1)
            foreach (var subAffix in SubAffixes)
                relic.SubAffixList.Add(subAffix.ToProto());

        return relic;
    }

    public ItemData Clone()
    {
        return new ItemData
        {
            UniqueId = UniqueId,
            ItemId = ItemId,
            Count = Count,
            Level = Level,
            Exp = Exp,
            TotalExp = TotalExp,
            Promotion = Promotion,
            Rank = Rank,
            Locked = Locked,
            Discarded = Discarded,
            MainAffix = MainAffix,
            SubAffixes = SubAffixes.Select(x => x.Clone()).ToList(),
            EquipAvatar = EquipAvatar
        };
    }

    #endregion
}

public class ItemSubAffix
{
    public ItemSubAffix()
    {
    }

    public ItemSubAffix(RelicSubAffixConfigExcel excel, int count)
    {
        Id = excel.AffixID;
        Count = count;
        Step = Extensions.RandomInt(0, excel.StepNum * count + 1);
    }

    public ItemSubAffix(int id, int count, int step)
    {
        Id = id;
        Count = count;
        Step = step;
    }

    public int Id { get; set; } // Affix id

    public int Count { get; set; }
    public int Step { get; set; }

    public void IncreaseStep(int stepNum)
    {
        Count++;
        Step += Extensions.RandomInt(0, stepNum + 1);
    }

    public RelicAffix ToProto()
    {
        return new RelicAffix
        {
            AffixId = (uint)Id,
            Cnt = (uint)Count,
            Step = (uint)Step
        };
    }

    public ItemSubAffix Clone()
    {
        return new ItemSubAffix
        {
            Id = Id,
            Count = Count,
            Step = Step
        };
    }
}