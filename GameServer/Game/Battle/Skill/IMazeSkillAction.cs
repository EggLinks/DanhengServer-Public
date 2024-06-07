using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Battle.Skill
{
    public interface IMazeSkillAction
    {
        public void OnCast(AvatarSceneInfo avatar);

        public void OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities);

        public void OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities);
    }
}
