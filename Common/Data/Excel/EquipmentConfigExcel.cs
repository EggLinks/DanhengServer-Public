using EggLink.DanhengServer.Enums.Item;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("EquipmentConfig.json")]
public class EquipmentConfigExcel : ExcelResource
{
    public int EquipmentID { get; set; }
    public bool Release { get; set; }
    public int ExpType { get; set; }
    public int MaxPromotion { get; set; } = 0;
    public int MaxRank { get; set; } = 0;

    [JsonConverter(typeof(StringEnumConverter))]
    public RarityEnum Rarity { get; set; } = 0;

    public override int GetId()
    {
        return EquipmentID;
    }

    public override void Loaded()
    {
        if (Release == false) return;
        GameData.EquipmentConfigData.Add(EquipmentID, this);
    }
}