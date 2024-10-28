using EggLink.DanhengServer.Data.Config.Task;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.SummonUnit;

public class SummonUnitConfigInfo
{
    public string GroupConfigName { get; set; } = "";
    public string ConfigEntityPath { get; set; } = "";

    public string TickLodTemplateName { get; set; } = "";

    //public TriggerEffect[] ResidentEffects { get; set; }
    public string ShoesType { get; set; } = "";
    public bool ShowShadow { get; set; }
    public bool ColliderIsTrigger { get; set; }

    public string AttachPoint { get; set; } = "";

    //public MVector3 LocalPosition { get; set; }
    //public MVector3 LocalRotation { get; set; }
    public DynamicFloat Duration { get; set; } = new();

    //public TaskConfigInfo[] OnCreate { get; set; }
    //public TaskConfigInfo[] OnDestroy { get; set; }
    //public TaskConfigInfo[] OnBeReplaced { get; set; }
    //public TaskConfigInfo[] OnHide { get; set; }
    //public TaskConfigInfo[] OnShow { get; set; }
    //public TaskConfigInfo[] OnGroundInvalid { get; set; }
    //public TaskConfigInfo[] OnResetPos { get; set; }
    //public TaskConfigInfo[] OnSummonerGroundMove { get; set; }
    //public SummonUnitCharacterAnimConfig AnimConfig { get; set; }
    public SummonUnitTriggerConfigInfo TriggerConfig { get; set; } = new();
    //public SummonUnitMoveConfig MoveConfig { get; set; }
    //public SummonUnitAIConfig AIConfig { get; set; }
    //public SummonUnitSkillConfig SkillConfig { get; set; }
    //public SummonUnitBeAttackConfig BeAttackConfig { get; set; }
    //public SummonUnitGatherConfig GatherConfig { get; set; }
    //public GOFKFHJBFHP DynamicValues { get; set; }


    public static SummonUnitConfigInfo LoadFromJsonObject(JObject obj)
    {
        SummonUnitConfigInfo info = new();
        if (obj.TryGetValue(nameof(GroupConfigName), out var value))
            info.GroupConfigName = value.ToString();
        if (obj.TryGetValue(nameof(ConfigEntityPath), out value))
            info.ConfigEntityPath = value.ToString();
        if (obj.TryGetValue(nameof(TickLodTemplateName), out value))
            info.TickLodTemplateName = value.ToString();
        if (obj.TryGetValue(nameof(ShoesType), out value))
            info.ShoesType = value.ToString();
        if (obj.TryGetValue(nameof(ShowShadow), out value))
            info.ShowShadow = value.Value<bool>();
        if (obj.TryGetValue(nameof(ColliderIsTrigger), out value))
            info.ColliderIsTrigger = value.Value<bool>();
        if (obj.TryGetValue(nameof(AttachPoint), out value))
            info.AttachPoint = value.ToString();
        if (obj.TryGetValue(nameof(Duration), out value))
            info.Duration = value.ToObject<DynamicFloat>() ?? new DynamicFloat();

        if (obj.TryGetValue(nameof(TriggerConfig), out value))
            info.TriggerConfig = SummonUnitTriggerConfigInfo.LoadFromJsonObject((value as JObject)!);
        return info;
    }
}