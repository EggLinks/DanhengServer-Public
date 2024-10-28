using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.Task;

public class RefreshMazeBuffTime : TaskConfigInfo
{
    public TargetEvaluator TargetType { get; set; } = new();
    public int ID { get; set; }
    public DynamicFloat LifeTime { get; set; } = new();


    public new static TaskConfigInfo LoadFromJsonObject(JObject obj)
    {
        var info = new RefreshMazeBuffTime
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

        if (obj.TryGetValue(nameof(LifeTime), out value)) info.LifeTime = value.ToObject<DynamicFloat>()!;

        return info;
    }
}