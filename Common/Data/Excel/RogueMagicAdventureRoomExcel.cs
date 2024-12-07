using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMagicAdventureRoom.json")]
public class RogueMagicAdventureRoomExcel : ExcelResource
{
    public int ParamGroupID { get; set; }
    public int RoomID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueAdventureGameplayTypeEnum AdventureType { get; set; }

    public override int GetId()
    {
        return RoomID;
    }

    public override void Loaded()
    {
        GameData.RogueMagicAdventureRoomData.Add(RoomID, this);
    }
}