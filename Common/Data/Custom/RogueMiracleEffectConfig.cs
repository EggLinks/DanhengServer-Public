using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Custom;

public class RogueMiracleEffectConfig
{
    public Dictionary<int, RogueMiracleEffect> Miracles { get; set; } = [];
}

public class RogueMiracleEffect
{
    public int MiracleId { get; set; }
    public int MaxDurability { get; set; }
    public Dictionary<int, RogueEffect> Effects { get; set; } = [];
}

public class RogueEffect
{
    public int EffectId { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueMiracleEffectTypeEnum Type { get; set; }

    public string DynamicKey { get; set; } = ""; // for arguments

    public List<int> Params { get; set; } = [];
}