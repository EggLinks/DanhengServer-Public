using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.AdventureAbility;

public class AdventureModifierLookupTableConfig
{
    public Dictionary<string, AdventureModifierConfig> ModifierMap { get; set; } = [];

    public static AdventureModifierLookupTableConfig LoadFromJObject(JObject obj)
    {
        var info = new AdventureModifierLookupTableConfig();

        if (!obj.ContainsKey(nameof(ModifierMap))) return info;
        foreach (var jObject in obj[nameof(ModifierMap)]!.ToObject<Dictionary<string, JObject>>()!)
            info.ModifierMap.Add(jObject.Key, AdventureModifierConfig.LoadFromJObject(jObject.Value));
        return info;
    }
}