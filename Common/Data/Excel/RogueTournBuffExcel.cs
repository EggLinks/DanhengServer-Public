using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournBuff.json")]
public class RogueTournBuffExcel : ExcelResource
{
    public int MazeBuffID { get; set; }
    public int MazeBuffLevel { get; set; }
    public int RogueBuffType { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public RogueBuffCategoryEnum RogueBuffCategory { get; set; }
    public int RogueBuffTag { get; set; }

    public bool IsInHandbook { get; set; }

    public override int GetId()
    {
        return MazeBuffID * 100 + MazeBuffLevel;
    }

    public override void Loaded()
    {
        GameData.RogueTournBuffData.TryAdd(GetId(), this);
    }
}