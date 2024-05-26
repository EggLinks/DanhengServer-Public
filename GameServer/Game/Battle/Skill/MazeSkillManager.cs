using EggLink.DanhengServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Battle.Skill
{
    public static class MazeSkillManager
    {
        public static MazeSkill GetSkill(int baseAvatarId, int skillIndex)
        {
            GameData.AvatarConfigData.TryGetValue(baseAvatarId, out var avatarConfig);
            MazeSkill mazeSkill = new([]);
            if (avatarConfig == null) return mazeSkill;

            if (skillIndex == 0)
            {
                // normal atk
               mazeSkill = new(avatarConfig.MazeAtk?.OnStart.ToList() ?? [], false, avatarConfig);
            }
            else
            {
                // maze skill
                mazeSkill = new(avatarConfig.MazeSkill?.OnStart.ToList() ?? [], true, avatarConfig);
            }
            return mazeSkill;
        }
    }
}
