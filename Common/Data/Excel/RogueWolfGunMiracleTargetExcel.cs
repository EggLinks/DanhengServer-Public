using EggLink.DanhengServer.Enums.Scene;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueWolfGunMiracleTarget.json")]
public class RogueWolfGunMiracleTargetExcel : ExcelResource
{
    public int MiracleID { get; set; }
    public int Basement { get; set; }
    public int LayerMiddle { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public GameModeTypeEnum GameMode { get; set; }

    public override int GetId()
    {
        return MiracleID;
    }

    public override void Loaded()
    {
        GameData.RogueWolfGunMiracleTargetData.Add(MiracleID, this);
    }
}