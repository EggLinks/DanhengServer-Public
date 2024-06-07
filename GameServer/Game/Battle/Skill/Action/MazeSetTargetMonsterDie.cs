using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Battle.Skill.Action
{
    public class MazeSetTargetMonsterDie : IMazeSkillAction
    {
        public void OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.MonsterData.Rank < Enums.MonsterRankEnum.Elite)
                {
                    entity.Kill();

                    entity.Scene.Player.LineupManager!.CostMp(1);
                    entity.Scene.Player.RogueManager!.GetRogueInstance()?.RollBuff(1);
                    entity.Scene.Player.RogueManager!.GetRogueInstance()?.GainMoney(Random.Shared.Next(20, 60));
                }
            }
        }

        public void OnCast(AvatarSceneInfo avatar)
        {
            
        }

        public void OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities)
        {
        }
    }
}
