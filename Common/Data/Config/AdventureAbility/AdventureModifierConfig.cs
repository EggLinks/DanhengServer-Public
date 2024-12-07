using EggLink.DanhengServer.Data.Config.Task;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.AdventureAbility;

public class AdventureModifierConfig
{
    public float LifeTime { get; set; }
    public int Level { get; set; }
    public int LevelMax { get; set; }
    public bool IsCountDownAfterBattle { get; set; }

    public bool ApplyBehaviorFlagBindEffects { get; set; }

    //public AdventureModifierBehaviorFlag[] BehaviorFlagList{ get; set; }     
    public float TickInterval { get; set; }
    public List<TaskConfigInfo> OnInterval { get; set; } = [];
    public List<TaskConfigInfo> OnAdd { get; set; } = [];
    public List<TaskConfigInfo> OnCreate { get; set; } = [];
    public List<TaskConfigInfo> OnDestroy { get; set; } = [];
    public List<TaskConfigInfo> OnStack { get; set; } = [];
    public List<TaskConfigInfo> OnAttack { get; set; } = [];
    public List<TaskConfigInfo> OnBeforeBattle { get; set; } = [];
    public List<TaskConfigInfo> OnAfterBattle { get; set; } = [];
    public List<TaskConfigInfo> OnStage { get; set; } = [];
    public List<TaskConfigInfo> OnUnstage { get; set; } = [];
    public List<TaskConfigInfo> OnForeGround { get; set; } = [];
    public List<TaskConfigInfo> OnBackGround { get; set; } = [];
    public List<TaskConfigInfo> OnStageByStory { get; set; } = [];
    public List<TaskConfigInfo> OnNpcMonsterCreate { get; set; } = [];
    public List<TaskConfigInfo> OnTeamLeaderChange { get; set; } = [];
    public List<TaskConfigInfo> OnBeforeAttack { get; set; } = [];
    public List<TaskConfigInfo> OnBeAttack { get; set; } = [];
    public List<TaskConfigInfo> OnModifierAdd { get; set; } = [];
    public List<TaskConfigInfo> OnModifierRemove { get; set; } = [];
    public List<TaskConfigInfo> OnOwnerBeKilled { get; set; } = [];
    public List<TaskConfigInfo> OnAfterLocalPlayerUseSkill { get; set; } = [];
    public List<TaskConfigInfo> ModifierTaskList { get; set; } = [];

    public List<TaskConfigInfo> OnCounterAttack { get; set; } = [];

    //public MazeBuffType MazeBuffType { get; set; }     
    public int Priority { get; set; }

    public int Count { get; set; }
    //public ModifierStacking Stacking { get; set; }


    public static AdventureModifierConfig LoadFromJObject(JObject obj)
    {
        var info = new AdventureModifierConfig();

        if (obj.ContainsKey(nameof(Level)))
            info.Level = obj[nameof(Level)]!.ToObject<int>();

        if (obj.ContainsKey(nameof(LevelMax)))
            info.LevelMax = obj[nameof(LevelMax)]!.ToObject<int>();

        if (obj.ContainsKey(nameof(LifeTime)))
            info.LifeTime = obj[nameof(LifeTime)]!.ToObject<float>();

        if (obj.ContainsKey(nameof(Count)))
            info.Count = obj[nameof(Count)]!.ToObject<int>();

        if (obj.ContainsKey(nameof(TickInterval)))
            info.TickInterval = obj[nameof(TickInterval)]!.ToObject<float>();

        if (obj.ContainsKey(nameof(IsCountDownAfterBattle)))
            info.IsCountDownAfterBattle = obj[nameof(IsCountDownAfterBattle)]!.ToObject<bool>();

        if (obj.ContainsKey(nameof(ApplyBehaviorFlagBindEffects)))
            info.ApplyBehaviorFlagBindEffects = obj[nameof(ApplyBehaviorFlagBindEffects)]!.ToObject<bool>();

        if (obj.ContainsKey(nameof(OnInterval)))
            info.OnInterval = obj[nameof(OnInterval)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnAdd)))
            info.OnAdd = obj[nameof(OnAdd)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnCreate)))
            info.OnCreate = obj[nameof(OnCreate)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnDestroy)))
            info.OnDestroy = obj[nameof(OnDestroy)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnStack)))
            info.OnStack = obj[nameof(OnStack)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnAttack)))
            info.OnAttack = obj[nameof(OnAttack)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnBeforeBattle)))
            info.OnBeforeBattle = obj[nameof(OnBeforeBattle)]
                ?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnAfterBattle)))
            info.OnAfterBattle = obj[nameof(OnAfterBattle)]
                ?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnStage)))
            info.OnStage = obj[nameof(OnStage)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnUnstage)))
            info.OnUnstage = obj[nameof(OnUnstage)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnForeGround)))
            info.OnForeGround = obj[nameof(OnForeGround)]
                ?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        if (obj.ContainsKey(nameof(OnBackGround)))
            info.OnBackGround = obj[nameof(OnBackGround)]
                ?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!))
                .ToList() ?? [];

        // TODO: others

        return info;
    }
}