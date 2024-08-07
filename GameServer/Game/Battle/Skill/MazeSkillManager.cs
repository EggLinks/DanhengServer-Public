using EggLink.DanhengServer.Data;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill;

public static class MazeSkillManager
{
    public static MazeSkill GetSkill(int baseAvatarId, int skillIndex)
    {
        GameData.AvatarConfigData.TryGetValue(baseAvatarId, out var avatarConfig);
        MazeSkill mazeSkill = new([]);
        if (avatarConfig == null) return mazeSkill;

        if (skillIndex == 0)
            // normal atk
            mazeSkill = new MazeSkill(avatarConfig.MazeAtk?.OnStart.ToList() ?? [], false, avatarConfig);
        else
            // maze skill
            mazeSkill = new MazeSkill(avatarConfig.MazeSkill?.OnStart.ToList() ?? [], true, avatarConfig);
        return mazeSkill;
    }
}