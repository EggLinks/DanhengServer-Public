using EggLink.DanhengServer.Enums.RogueMagic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMagicScepter.json")]
public class RogueMagicScepterExcel : ExcelResource
{
    public int ScepterID { get; set; }
    public int ScepterLevel { get; set; }
    public List<LockMagicUnitInfo> LockMagicUnit { get; set; } = [];
    public Dictionary<RogueMagicMountTypeEnum, int> TrenchCount { get; set; } = [];
    public int UnlockID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueMagicScepterFuncTypeEnum FuncType { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueMagicStyleTypeEnum StyleType { get; set; }

    //public FixPoint ScepterBasicPower { get; set; }
    public int StaffMazeBuffID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueMagicRangeTypeEnum LimitRangeType { get; set; }

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<RogueMagicEffectTypeEnum> EffectTypeList { get; set; } = [];

    public override int GetId()
    {
        return ScepterID * 100 + ScepterLevel;
    }

    public override void Loaded()
    {
        GameData.RogueMagicScepterData.Add(GetId(), this);
    }
}

public class LockMagicUnitInfo
{
    [JsonProperty("GHFHMJLCIEC")] public int MagicUnitId { get; set; }

    [JsonProperty("LDEDAMNEIJO")] public int MagicUnitLevel { get; set; }
}