using EggLink.DanhengServer.Enums.Item;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MappingInfo.json")]
public class MappingInfoExcel : ExcelResource
{
    public int ID { get; set; }
    public int WorldLevel { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public FarmTypeEnum FarmType { get; set; } = FarmTypeEnum.None; // is enum

    public List<MappingInfoItem> DisplayItemList { get; set; } = [];

    [JsonIgnore] public List<MappingInfoItem> DropItemList { get; set; } = [];

    [JsonIgnore] public List<MappingInfoItem> DropRelicItemList { get; set; } = [];

    public override int GetId()
    {
        return ID * 10 + WorldLevel;
    }

    public override void Loaded()
    {
        GameData.MappingInfoData.Add(GetId(), this);
        if (DisplayItemList.Count == 0) return;

        List<int> equipDrop = [];
        Dictionary<int, List<int>> relicDrop = [];

        foreach (var item in DisplayItemList)
        {
            if (item.ItemNum > 0)
            {
                DropItemList.Add(item);
                continue;
            }

            if (item.ItemID == 2)
            {
                DropItemList.Add(new MappingInfoItem() // random credit
                {
                    ItemID = 2,
                    MinCount = (50 + WorldLevel * 10) * (int)FarmType,
                    MaxCount = (100 + WorldLevel * 10) * (int)FarmType
                });

                continue;
            }

            GameData.ItemConfigData.TryGetValue(item.ItemID, out var excel);
            if (excel == null) continue;

            if (excel.ItemSubType == ItemSubTypeEnum.RelicSetShowOnly)
            {
                var baseRelicId = item.ItemID / 10 % 1000;
                var baseRarity = item.ItemID % 10;

                // Add relics from the set
                var relicStart = 20001 + baseRarity * 10000 + baseRelicId * 10;
                var relicEnd = relicStart + 3;
                for (; relicStart <= relicEnd; relicStart++)
                {
                    GameData.ItemConfigData.TryGetValue(relicStart, out var relicExcel);
                    if (relicExcel == null) break;


                    if (!relicDrop.TryGetValue(baseRarity, out var _))
                    {
                        var value = new List<int>();
                        relicDrop[baseRarity] = value;
                    }

                    relicDrop[baseRarity].Add(relicStart);
                }
            }
            else if (excel.ItemMainType == ItemMainTypeEnum.Material)
            {
                // Calculate amount to drop by purpose level
                MappingInfoItem? drop;
                switch (excel.PurposeType)
                {
                    // Avatar exp. Drop rate is guessed (with data)
                    case 1:
                        // Calc amount
                        var amount = excel.Rarity switch
                        {
                            ItemRarityEnum.NotNormal => WorldLevel < 3 ? WorldLevel + 3 : 2.5,
                            ItemRarityEnum.Rare => WorldLevel < 3 ? WorldLevel + 3 : WorldLevel * 2 - 3,
                            _ => 1
                        };

                        drop = new MappingInfoItem(excel.ID, (int)amount);
                        break;
                    // Boss materials
                    case 2:
                        drop = new MappingInfoItem(excel.ID, WorldLevel);
                        break;
                    // Trace materials. Drop rate is guessed (with data)
                    case 3:
                        drop = new MappingInfoItem(excel.ID, 5);
                        break;
                    // Boss Trace materials. Drop rate is guessed (with data)
                    case 4:
                        drop = new MappingInfoItem(excel.ID, (int)(WorldLevel * 0.5 + 0.5));
                        break;
                    // Lightcone exp. Drop rate is guessed (with data)
                    case 5:
                        // Calc amount
                        var count = excel.Rarity switch
                        {
                            ItemRarityEnum.NotNormal => Math.Max(5 - WorldLevel, 2.5),
                            ItemRarityEnum.Rare => WorldLevel % 3 + 1,
                            _ => 1
                        };

                        drop = new MappingInfoItem(excel.ID, (int)count);
                        break;
                    // Lucent afterglow
                    case 11:
                        drop = new MappingInfoItem(excel.ID, 4 + WorldLevel);
                        break;
                    // Unknown
                    default:
                        drop = null;
                        break;
                }

                ;

                // Add to drop list
                if (drop != null) DropItemList.Add(drop);
            }
            else if (excel.ItemMainType == ItemMainTypeEnum.Equipment)
            {
                // Add lightcones
                equipDrop.Add(excel.ID);
            }


            // Add equipment drops
            if (equipDrop.Count > 0)
                foreach (var dropId in equipDrop)
                {
                    MappingInfoItem drop = new(dropId, 1)
                    {
                        Chance = WorldLevel * 10 + 40
                    };
                    DropItemList.Add(drop);
                }

            // Add relic drops
            if (relicDrop.Count > 0)
                foreach (var entry in relicDrop)
                    // Add items to drop param
                foreach (var value in entry.Value)
                {
                    MappingInfoItem drop = new(value, 1);

                    // Set count by rarity
                    var amount = entry.Key switch
                    {
                        4 =>
                            WorldLevel * 0.5 - 0.5,
                        3 =>
                            WorldLevel * 0.5 + (WorldLevel == 2 ? 1.0 : 0),
                        2 =>
                            6 - WorldLevel + 0.5 - (WorldLevel == 1 ? 3.75 : 0),
                        _ =>
                            WorldLevel == 1 ? 6 : 2
                    };

                    // Set amount
                    if (amount > 0)
                    {
                        drop.ItemNum = (int)amount;
                        DropRelicItemList.Add(drop);
                    }
                }
        }
    }
}

public class MappingInfoItem
{
    public MappingInfoItem()
    {
    }

    public MappingInfoItem(int itemId, int itemNum)
    {
        ItemID = itemId;
        ItemNum = itemNum;
    }

    public int ItemID { get; set; }
    public int ItemNum { get; set; }

    public int MinCount { get; set; }
    public int MaxCount { get; set; }
    public int Chance { get; set; } = 100;
}