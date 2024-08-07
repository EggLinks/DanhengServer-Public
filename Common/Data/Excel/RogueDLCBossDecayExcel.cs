using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueDLCBossDecay.json")]
public class RogueDLCBossDecayExcel : ExcelResource
{
    public int BossDecayID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public BossDecayEffectTypeEnum EffectType { get; set; }

    public List<int> EffectParamList { get; set; } = [];

    public override int GetId()
    {
        return BossDecayID;
    }

    public override void Loaded()
    {
        GameData.RogueDLCBossDecayData.Add(GetId(), this);
    }
}