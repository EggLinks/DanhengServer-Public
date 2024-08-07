using EggLink.DanhengServer.Enums.Item;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ItemConfig.json,ItemConfigAvatar.json,ItemConfigAvatarPlayerIcon.json," +
                "ItemConfigAvatarRank.json,ItemConfigBook.json,ItemConfigDisk.json," +
                "ItemConfigEquipment.json,ItemConfigRelic.json,ItemPlayerCard.json", true)]
public class ItemConfigExcel : ExcelResource
{
    // General item data
    public int ID { get; set; }
    public HashName ItemName { get; set; } = new();

    [JsonConverter(typeof(StringEnumConverter))]
    public ItemMainTypeEnum ItemMainType { get; set; } = ItemMainTypeEnum.Unknown;

    [JsonConverter(typeof(StringEnumConverter))]
    public ItemSubTypeEnum ItemSubType { get; set; } = ItemSubTypeEnum.Unknown;

    [JsonConverter(typeof(StringEnumConverter))]
    public ItemRarityEnum Rarity { get; set; } = ItemRarityEnum.Unknown;

    public int PileLimit { get; set; }
    public int PurposeType { get; set; }

    public int UseDataID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ItemUseMethodEnum UseMethod { get; set; }

    public List<MappingInfoItem> ReturnItemIDList { get; set; } = [];

    [JsonIgnore] public int Exp { get; set; }

    [JsonIgnore] public string? Name { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.ItemConfigData[ID] = this;
    }
}