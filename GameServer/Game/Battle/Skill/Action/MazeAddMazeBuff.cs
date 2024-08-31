using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill.Action;

public class MazeAddMazeBuff(int buffId, int duration) : IMazeSkillAction
{
    public int BuffId { get; } = buffId;

    public async ValueTask OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        foreach (var entity in entities)
            entity.TempBuff = new SceneBuff(BuffId, 1, avatar.AvatarInfo.AvatarId, duration);

        await System.Threading.Tasks.Task.CompletedTask;
    }

    public async ValueTask OnCast(AvatarSceneInfo avatar, PlayerInstance player)
    {
        await avatar.AddBuff(new SceneBuff(BuffId, 1, avatar.AvatarInfo.AvatarId, duration));
    }

    public async ValueTask OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        foreach (var entity in entities)
            await entity.AddBuff(new SceneBuff(BuffId, 1, avatar.AvatarInfo.AvatarId, duration));
    }
}