using EggLink.DanhengServer.Enums.TournRogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Custom;

public class RogueTournRoomConfig
{
    public int EntranceId { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTournRoomTypeEnum RoomType { get; set; }

    public int AnchorGroup { get; set; }
    public int AnchorId { get; set; }

    public List<int> DefaultLoadBasicGroup { get; set; } = [];
    public List<int> DefaultLoadGroup { get; set; } = [];

    public List<int> SubMonsterGroup { get; set; } = []; // combine with DefaultLoadGroup

    public RogueTournRoomConfig Clone(RogueTournRoomTypeEnum type)
    {
        return new RogueTournRoomConfig
        {
            RoomType = type,
            AnchorGroup = AnchorGroup,
            AnchorId = AnchorId,
            DefaultLoadBasicGroup = DefaultLoadBasicGroup,
            DefaultLoadGroup = DefaultLoadGroup,
            EntranceId = EntranceId,
            SubMonsterGroup = SubMonsterGroup
        };
    }
}