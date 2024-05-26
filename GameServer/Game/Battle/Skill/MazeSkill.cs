using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.Game.Battle.Skill.Action;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Battle.Skill
{
    public class MazeSkill
    {
        public List<IMazeSkillAction> Actions { get; private set; } = [];
        public bool TriggerBattle { get; private set; } = true;
        public bool IsMazeSkill { get; private set; } = true;
        public AvatarConfigExcel? Excel { get; private set; }

        public MazeSkill(List<TaskInfo> taskInfos, bool isSkill = false, AvatarConfigExcel? excel = null)
        {
            foreach (var task in taskInfos)
            {
                AddAction(task);
            }
            IsMazeSkill = isSkill;
            Excel = excel;
        }

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
                    Actions.RemoveAll(a => a is MazeAddMazeBuff buff && buff.BuffId == task.ID);
                    break;
                case TaskTypeEnum.AdventureModifyTeamPlayerHP:
                    break;
                case TaskTypeEnum.AdventureModifyTeamPlayerSP:
                    break;
                case TaskTypeEnum.CreateSummonUnit:
                    break;
                case TaskTypeEnum.AdventureSetAttackTargetMonsterDie:
                    Actions.Add(new MazeSetTargetMonsterDie());
                    break;
                case TaskTypeEnum.SuccessTaskList:
                    foreach (var t in task.SuccessTaskList)
                    {
                        AddAction(t);
                    }
                    break;
                case TaskTypeEnum.AdventureTriggerAttack:
                    if (IsMazeSkill)
                    {
                        TriggerBattle = task.TriggerBattle;
                    }

                    foreach (var t in task.GetAttackInfo())
                    {
                        AddAction(t);
                    }
                    break;
                case TaskTypeEnum.AdventureFireProjectile:
                    foreach (var t in task.OnProjectileHit)
                    {
                        AddAction(t);
                    }

                    foreach (var t in task.OnProjectileLifetimeFinish)
                    {
                        AddAction(t);
                    }
                    break;
            }
        }

        public void OnCast(AvatarSceneInfo info)
        {
            foreach (var action in Actions)
            {
                action.OnCast(info);
            }
        }

        public void OnAttack(AvatarSceneInfo info, List<EntityMonster> entities)
        {
            foreach (var action in Actions)
            {
                action.OnAttack(info, entities);
            }
        }

        public void OnHitTarget(AvatarSceneInfo info, List<EntityMonster> entities)
        {
            foreach (var action in Actions)
            {
                action.OnHitTarget(info, entities);
            }
        }
    }
}
