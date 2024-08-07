using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Custom;

public class ChessRogueRoomConfig
{
    public int EntranceId { get; set; }
    public int RoomPrefix { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueDLCBlockTypeEnum BlockType { get; set; }

    public int AnchorGroup { get; set; }
    public int AnchorId { get; set; }

    public List<int> DefaultLoadBasicGroup { get; set; } = [];
    public List<int> DefaultLoadGroup { get; set; } = [];
    public List<int> DoubleEventLoadGroup { get; set; } = []; // override DefaultLoadGroup
    public List<int> SelectEventLoadGroup { get; set; } = []; // override DefaultLoadGroup

    public List<int> SubMonsterGroup { get; set; } = []; // combine with DefaultLoadGroup
}