using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill;

public interface IMazeSkillAction
{
    public ValueTask OnCast(AvatarSceneInfo avatar, PlayerInstance player);

    public ValueTask OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities);

    public ValueTask OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities);
}