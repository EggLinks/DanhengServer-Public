using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill;

public static class MazeSkillManager
{
    public static MazeSkill GetSkill(int baseAvatarId, int skillIndex, SceneCastSkillCsReq req)
    {
        GameData.AvatarConfigData.TryGetValue(baseAvatarId, out var avatarConfig);
        MazeSkill mazeSkill = new([], req);
        if (avatarConfig == null) return mazeSkill;

        if (skillIndex == 0)
            // normal atk
            mazeSkill = new MazeSkill(avatarConfig.MazeAtk?.OnStart.ToList() ?? [], req, false, avatarConfig);
        else
            // maze skill
            mazeSkill = new MazeSkill(avatarConfig.MazeSkill?.OnStart.ToList() ?? [], req, true, avatarConfig);
        return mazeSkill;
    }

    public static MazeSkill GetSkill(int baseAvatarId, AbilityInfo ability, SceneCastSkillCsReq req)
    {
        GameData.AvatarConfigData.TryGetValue(baseAvatarId, out var avatarConfig);
        MazeSkill mazeSkill = new([], req);
        if (avatarConfig == null) return mazeSkill;

        mazeSkill = new MazeSkill(ability.OnStart, req, true, avatarConfig);
        return mazeSkill;
    }
}