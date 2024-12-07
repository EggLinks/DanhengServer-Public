using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Custom;

public abstract class BaseRogueBuffExcel : ExcelResource
{
    public int MazeBuffID { get; set; }
    public int MazeBuffLevel { get; set; }
    public int RogueBuffType { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueBuffCategoryEnum RogueBuffCategory { get; set; }

    public int RogueBuffTag { get; set; }

    public RogueCommonBuff ToProto()
    {
        return new RogueCommonBuff
        {
            BuffId = (uint)MazeBuffID,
            BuffLevel = (uint)MazeBuffLevel
        };
    }
}