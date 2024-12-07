using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTurntable.json")]
public class RogueTurntableExcel : ExcelResource
{
    public int ParamGroupID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTurntableRewardLevelEnum RewardLevel { get; set; }

    public int PrepareTime { get; set; }

    public override int GetId()
    {
        return ParamGroupID * 10 + (int)RewardLevel;
    }

    public override void Loaded()
    {
        GameData.RogueTurntableData.Add(GetId(), this);
    }
}