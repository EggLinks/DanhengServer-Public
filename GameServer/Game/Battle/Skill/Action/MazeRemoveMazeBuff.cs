using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill.Action;

public class MazeRemoveMazeBuff(int buffId) : IMazeSkillAction
{
    public int BuffId { get; } = buffId;

    public async ValueTask OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        await System.Threading.Tasks.Task.CompletedTask;
    }

    public async ValueTask OnCast(AvatarSceneInfo avatar, PlayerInstance player)
    {
        await avatar.RemoveBuff(BuffId);
    }

    public async ValueTask OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        foreach (var entity in entities)
            await entity.RemoveBuff(BuffId);
    }
}