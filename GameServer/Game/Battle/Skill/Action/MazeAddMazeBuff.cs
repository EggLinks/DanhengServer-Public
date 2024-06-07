using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Battle.Skill.Action
{
    public class MazeAddMazeBuff(int buffId, int duration) : IMazeSkillAction
    {
        public int BuffId { get; private set; } = buffId;

        public void OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities)
        {
            foreach (var entity in entities)
            {
                entity.TempBuff = new SceneBuff(BuffId, 1, avatar.AvatarInfo.AvatarId, duration);
            }
        }

        public void OnCast(AvatarSceneInfo avatar)
        {
            avatar.AddBuff(new SceneBuff(BuffId, 1, avatar.AvatarInfo.AvatarId, duration));
        }

        public void OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities)
        {
            foreach (var entity in entities)
            {
                entity.AddBuff(new SceneBuff(BuffId, 1, avatar.AvatarInfo.AvatarId, duration));
            }
        }
    }
}
