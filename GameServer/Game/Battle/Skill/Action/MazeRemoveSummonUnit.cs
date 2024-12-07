using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill.Action;

public class MazeRemoveSummonUnit(int unitId) : IMazeSkillAction
{
    public int SummonUnitId = unitId;

    public async ValueTask OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        await System.Threading.Tasks.Task.CompletedTask;
    }

    public async ValueTask OnCast(AvatarSceneInfo avatar, PlayerInstance player)
    {
        if (player.SceneInstance!.SummonUnit?.SummonUnitId == SummonUnitId)
            // remove
            await player.SceneInstance!.ClearSummonUnit();
    }

    public async ValueTask OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        await System.Threading.Tasks.Task.CompletedTask;
    }
}