using EggLink.DanhengServer.Enums.TournRogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournRoom.json")]
public class RogueTournRoomExcel : ExcelResource
{
    public int RogueRoomID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTournRoomTypeEnum RogueRoomType { get; set; }


    public override int GetId()
    {
        return RogueRoomID;
    }

    public override void Loaded()
    {
        GameData.RogueTournRoomData.Add(RogueRoomID, this);
    }
}