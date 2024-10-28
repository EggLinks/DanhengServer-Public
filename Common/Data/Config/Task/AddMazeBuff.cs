using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.Task;

public class AddMazeBuff : TaskConfigInfo
{
    public TargetEvaluator TargetType { get; set; } = new();
    public int ID { get; set; }
    public PredicateConfigInfo Condition { get; set; } = new();
    public TargetEvaluator BindingAffectedTarget { get; set; } = new();
    public DynamicFloat LifeTime { get; set; } = new();
    public DynamicFloat Count { get; set; } = new();
    public DynamicFloat Level { get; set; } = new();
    public Dictionary<string, DynamicFloat> DynamicValues { get; set; } = [];

    public new static TaskConfigInfo LoadFromJsonObject(JObject obj)
    {
        var info = new AddMazeBuff
        {
            Type = obj[nameof(Type)]!.ToObject<string>()!
        };
        if (obj.TryGetValue(nameof(TargetType), out var value))
        {
            var targetType = value as JObject;
            var classType =
                System.Type.GetType(
                    $"EggLink.DanhengServer.Data.Config.Task.{targetType?["Type"]?.ToString().Replace("RPG.GameCore.", "")}");
            classType ??= System.Type.GetType("EggLink.DanhengServer.Data.Config.Task.TargetEvaluator");
            info.TargetType = (targetType!.ToObject(classType!) as TargetEvaluator)!;
        }

        if (obj.TryGetValue(nameof(ID), out value)) info.ID = value.ToObject<int>()!;
        if (obj.TryGetValue(nameof(Condition), out value))
        {
            var condition = value as JObject;
            var classType =
                System.Type.GetType(
                    $"EggLink.DanhengServer.Data.Config.Task.{condition?["Type"]?.ToString().Replace("RPG.GameCore.", "")}");
            classType ??= System.Type.GetType("EggLink.DanhengServer.Data.Config.Task.PredicateConfigInfo");
            info.Condition = (condition!.ToObject(classType!) as PredicateConfigInfo)!;
        }

        if (obj.TryGetValue(nameof(BindingAffectedTarget), out value))
        {
            var bindingAffectedTarget = value as JObject;
            var classType =
                System.Type.GetType(
                    $"EggLink.DanhengServer.Data.Config.Task.{bindingAffectedTarget?["Type"]?.ToString().Replace("RPG.GameCore.", "")}");
            classType ??= System.Type.GetType("EggLink.DanhengServer.Data.Config.Task.TargetEvaluator");
            info.BindingAffectedTarget = (bindingAffectedTarget!.ToObject(classType!) as TargetEvaluator)!;
        }

        if (obj.TryGetValue(nameof(LifeTime), out value)) info.LifeTime = value.ToObject<DynamicFloat>()!;

        if (obj.TryGetValue(nameof(Count), out value)) info.Count = value.ToObject<DynamicFloat>()!;
        if (obj.TryGetValue(nameof(Level), out value)) info.Level = value.ToObject<DynamicFloat>()!;

        if (!obj.TryGetValue(nameof(DynamicValues), out value)) return info;
        var dynamicValues = value as JObject;
        var dictionary = new Dictionary<string, DynamicFloat>();
        foreach (var (key, val) in dynamicValues!)
            dictionary[key] = val?.ToObject<DynamicFloat>()!;
        info.DynamicValues = dictionary;

        return info;
    }
}