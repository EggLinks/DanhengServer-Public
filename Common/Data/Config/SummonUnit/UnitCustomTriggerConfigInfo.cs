using EggLink.DanhengServer.Data.Config.Task;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.SummonUnit;

public class UnitCustomTriggerConfigInfo
{
    public string TriggerName { get; set; } = "";
    public bool DefaultEnable { get; set; }
    public DynamicFloat Radius { get; set; } = new();
    public bool UseVerticalBound { get; set; }
    public float UpperBound { get; set; }
    public float LowerBound { get; set; }
    public bool NeedRaycast { get; set; }
    public float RayStartOffsetY { get; set; }
    public bool ForceSyncTargetToSever { get; set; }
    public bool DependOnServerTarget { get; set; }

    public bool IsSingle { get; set; }

    // EntityType TargetEntityType { get; set; }
    public DynamicFloat TargetGroupID { get; set; } = new();

    public DynamicFloat TargetID { get; set; } = new();

    // EntityType[] TargetTypes { get; set; }
    // PredicateConfigInfo TargetFilter { get; set; }
    public string ColliderRelativePath { get; set; } = "";
    public bool DestroyAfterTriggered { get; set; }
    public bool DisableAfterTriggered { get; set; }
    public List<TaskConfigInfo> OnTriggerEnable { get; set; } = [];
    public List<TaskConfigInfo> OnTriggerDisable { get; set; } = [];
    public List<TaskConfigInfo> OnTriggerEnter { get; set; } = [];
    public List<TaskConfigInfo> OnTriggerExit { get; set; } = [];
    public List<TaskConfigInfo> OnTriggerEnterRollback { get; set; } = [];
    public bool BlockDialogueInRange { get; set; }
    public bool DestroyAfterGraphEnd { get; set; }
    public bool TriggerByFakeAvatar { get; set; }
    public bool SkipFakeAvatar { get; set; }

    public static UnitCustomTriggerConfigInfo LoadFromJsonObject(JObject obj)
    {
        UnitCustomTriggerConfigInfo info = new();
        if (obj.TryGetValue(nameof(TriggerName), out var value))
            info.TriggerName = value.ToString();

        if (obj.TryGetValue(nameof(DefaultEnable), out value))
            info.DefaultEnable = value.Value<bool>();

        if (obj.TryGetValue(nameof(Radius), out value))
            info.Radius = value.ToObject<DynamicFloat>() ?? new DynamicFloat();

        if (obj.TryGetValue(nameof(UseVerticalBound), out value))
            info.UseVerticalBound = value.Value<bool>();

        if (obj.TryGetValue(nameof(UpperBound), out value))
            info.UpperBound = value.Value<float>();

        if (obj.TryGetValue(nameof(LowerBound), out value))
            info.LowerBound = value.Value<float>();

        if (obj.TryGetValue(nameof(NeedRaycast), out value))
            info.NeedRaycast = value.Value<bool>();

        if (obj.TryGetValue(nameof(RayStartOffsetY), out value))
            info.RayStartOffsetY = value.Value<float>();

        if (obj.TryGetValue(nameof(ForceSyncTargetToSever), out value))
            info.ForceSyncTargetToSever = value.Value<bool>();

        if (obj.TryGetValue(nameof(DependOnServerTarget), out value))
            info.DependOnServerTarget = value.Value<bool>();

        if (obj.TryGetValue(nameof(IsSingle), out value))
            info.IsSingle = value.Value<bool>();

        if (obj.TryGetValue(nameof(TargetGroupID), out value))
            info.TargetGroupID = value.ToObject<DynamicFloat>() ?? new DynamicFloat();

        if (obj.TryGetValue(nameof(TargetID), out value))
            info.TargetID = value.ToObject<DynamicFloat>() ?? new DynamicFloat();

        if (obj.TryGetValue(nameof(ColliderRelativePath), out value))
            info.ColliderRelativePath = value.ToString();

        if (obj.TryGetValue(nameof(DestroyAfterTriggered), out value))
            info.DestroyAfterTriggered = value.Value<bool>();

        if (obj.TryGetValue(nameof(DisableAfterTriggered), out value))
            info.DisableAfterTriggered = value.Value<bool>();

        if (obj.TryGetValue(nameof(OnTriggerEnable), out value))
            info.OnTriggerEnable = value.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)).ToList();

        if (obj.TryGetValue(nameof(OnTriggerDisable), out value))
            info.OnTriggerDisable = value.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)).ToList();

        if (obj.TryGetValue(nameof(OnTriggerEnter), out value))
            info.OnTriggerEnter = value.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)).ToList();

        if (obj.TryGetValue(nameof(OnTriggerExit), out value))
            info.OnTriggerExit = value.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)).ToList();

        if (obj.TryGetValue(nameof(OnTriggerEnterRollback), out value))
            info.OnTriggerEnterRollback =
                value.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)).ToList();

        if (obj.TryGetValue(nameof(BlockDialogueInRange), out value))
            info.BlockDialogueInRange = value.Value<bool>();

        if (obj.TryGetValue(nameof(DestroyAfterGraphEnd), out value))
            info.DestroyAfterGraphEnd = value.Value<bool>();

        if (obj.TryGetValue(nameof(TriggerByFakeAvatar), out value))
            info.TriggerByFakeAvatar = value.Value<bool>();

        if (obj.TryGetValue(nameof(SkipFakeAvatar), out value))
            info.SkipFakeAvatar = value.Value<bool>();

        return info;
    }
}