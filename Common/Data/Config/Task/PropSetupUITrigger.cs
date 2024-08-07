using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.Task;

public class PropSetupUITrigger : TaskConfigInfo
{
    public string ColliderRelativePath { get; set; } = string.Empty;
    public bool DestroyAfterTriggered { get; set; }
    public bool DisableAfterTriggered { get; set; }
    public bool DisableWhenTriggered { get; set; }

    public string ButtonIcon { get; set; } = string.Empty;

    //DialogueIconType IconType;
    //TextID ButtonText;
    //DynamicString ButtonTextCustom;
    public List<TaskConfigInfo> ButtonCallback { get; set; } = [];
    public bool ForceInteractInDanger { get; set; }
    public bool ConsiderAngleLimit { get; set; }

    public float InteractAngleRange { get; set; }

    //EntityType[] OverrideTargetTypes;
    public bool TriggerByFakeAvatar { get; set; }
    public bool SkipFakeAvatar { get; set; }
    public PredicateConfigInfo OnEnterFilter { get; set; } = new();
    public TargetEvaluator TargetType { get; set; } = new();

    public new static TaskConfigInfo LoadFromJsonObject(JObject obj)
    {
        PropSetupUITrigger info = new();
        info.Type = obj[nameof(Type)]!.ToObject<string>()!;

        if (obj.ContainsKey(nameof(OnEnterFilter)))
            info.OnEnterFilter = PredicateConfigInfo.LoadFromJsonObject((obj[nameof(OnEnterFilter)] as JObject)!)!;

        if (obj.ContainsKey(nameof(ButtonCallback)))
            info.ButtonCallback = obj[nameof(ButtonCallback)]
                ?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)).ToList() ?? [];

        if (obj.ContainsKey(nameof(TargetType)))
        {
            var targetType = obj[nameof(TargetType)] as JObject;
            var classType =
                System.Type.GetType(
                    $"EggLink.DanhengServer.Data.Config.Task.{targetType?["Type"]?.ToString().Replace("RPG.GameCore.", "")}");
            classType ??= System.Type.GetType("EggLink.DanhengServer.Data.Config.Task.TargetEvaluator");
            info.TargetType = (targetType!.ToObject(classType!) as TargetEvaluator)!;
        }

        if (info.ButtonCallback.Count > 0 && info.ButtonCallback[0].Type == "RPG.GameCore.PropStateExecute")
            info.Type = obj[nameof(Type)]!.ToObject<string>()!;

        info.ColliderRelativePath = obj[nameof(ColliderRelativePath)]?.ToString() ?? string.Empty;
        info.DestroyAfterTriggered = obj[nameof(DestroyAfterTriggered)]?.ToObject<bool>() ?? false;
        info.DisableAfterTriggered = obj[nameof(DisableAfterTriggered)]?.ToObject<bool>() ?? false;
        info.DisableWhenTriggered = obj[nameof(DisableWhenTriggered)]?.ToObject<bool>() ?? false;
        info.ButtonIcon = obj[nameof(ButtonIcon)]?.ToString() ?? string.Empty;
        info.ForceInteractInDanger = obj[nameof(ForceInteractInDanger)]?.ToObject<bool>() ?? false;
        info.ConsiderAngleLimit = obj[nameof(ConsiderAngleLimit)]?.ToObject<bool>() ?? false;
        info.InteractAngleRange = obj[nameof(InteractAngleRange)]?.ToObject<float>() ?? 0;
        info.TriggerByFakeAvatar = obj[nameof(TriggerByFakeAvatar)]?.ToObject<bool>() ?? false;
        info.SkipFakeAvatar = obj[nameof(SkipFakeAvatar)]?.ToObject<bool>() ?? false;

        return info;
    }
}