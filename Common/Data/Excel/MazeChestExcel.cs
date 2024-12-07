using EggLink.DanhengServer.Enums.Scene;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MazeChest.json")]
public class MazeChestExcel : ExcelResource
{
    public int WorldID { get; set; }
    public int ID { get; set; }

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<ChestTypeEnum> ChestType { get; set; } = [];

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MazeChestData.Add(ID, this);
    }
}