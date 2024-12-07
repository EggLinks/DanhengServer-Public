using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.GameServer.Game.Battle.Skill.Action;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill;

public class MazeSkill
{
    public SceneCastSkillCsReq Req;

    public MazeSkill(List<TaskInfo> taskInfos, SceneCastSkillCsReq req, bool isSkill = false,
        AvatarConfigExcel? excel = null)
    {
        Req = req;
        IsMazeSkill = isSkill;
        Excel = excel;
        foreach (var task in taskInfos) AddAction(task);

        if (GameData.SummonUnitDataData.TryGetValue((excel?.AvatarID ?? 0) * 10 + 1, out var summonUnit) && isSkill &&
            !summonUnit.IsClient &&
            req.MazeAbilityStr == "") Actions.Add(new MazeSummonUnit(summonUnit, req.TargetMotion));
    }

    public List<IMazeSkillAction> Actions { get; } = [];
    public bool TriggerBattle { get; private set; } = true;
    public bool IsMazeSkill { get; } = true;
    public AvatarConfigExcel? Excel { get; private set; }

    public void AddAction(TaskInfo task)
    {
        switch (task.TaskType)
        {
            case TaskTypeEnum.None:
                break;
            case TaskTypeEnum.AddMazeBuff:
                Actions.Add(new MazeAddMazeBuff(task.ID, task.LifeTime.GetLifeTime()));
                break;
            case TaskTypeEnum.RemoveMazeBuff:
                Actions.Add(new MazeRemoveMazeBuff(task.ID));
                break;
            case TaskTypeEnum.AdventureModifyTeamPlayerHP:
                break;
            case TaskTypeEnum.AdventureModifyTeamPlayerSP:
                break;
            case TaskTypeEnum.CreateSummonUnit:
                //Actions.Add(new MazeSummonUnit(GameData.SummonUnitDataData[task.SummonUnitID], Req.TargetMotion));
                break;
            case TaskTypeEnum.DestroySummonUnit:
                Actions.Add(new MazeRemoveSummonUnit(task.SummonUnit.SummonUnitID));
                break;
            case TaskTypeEnum.AdventureSetAttackTargetMonsterDie:
                Actions.Add(new MazeSetTargetMonsterDie());
                break;
            case TaskTypeEnum.SuccessTaskList:
                foreach (var t in task.SuccessTaskList) AddAction(t);
                break;
            case TaskTypeEnum.AdventureTriggerAttack:
                if (IsMazeSkill) TriggerBattle = task.TriggerBattle;

                foreach (var t in task.GetAttackInfo()) AddAction(t);
                break;
            case TaskTypeEnum.AdventureFireProjectile:
                foreach (var t in task.OnProjectileHit) AddAction(t);

                foreach (var t in task.OnProjectileLifetimeFinish) AddAction(t);
                break;
        }
    }

    public void OnCast(AvatarSceneInfo info, PlayerInstance player)
    {
        foreach (var action in Actions) action.OnCast(info, player);
    }

    public void OnAttack(AvatarSceneInfo info, List<EntityMonster> entities)
    {
        foreach (var action in Actions) action.OnAttack(info, entities);
    }

    public void OnHitTarget(AvatarSceneInfo info, List<EntityMonster> entities)
    {
        foreach (var action in Actions) action.OnHitTarget(info, entities);
    }
}