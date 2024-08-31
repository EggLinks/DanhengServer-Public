using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill.Action;

public class MazeSetTargetMonsterDie : IMazeSkillAction
{
    public async ValueTask OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        foreach (var entity in entities)
            if (entity.MonsterData.Rank < MonsterRankEnum.Elite)
            {
                await entity.Kill();

                await entity.Scene.Player.LineupManager!.CostMp(1, (uint)avatar.EntityID);
                entity.Scene.Player.RogueManager!.GetRogueInstance()?.RollBuff(1);
                entity.Scene.Player.RogueManager!.GetRogueInstance()?.GainMoney(Random.Shared.Next(20, 60));
            }
    }

    public async ValueTask OnCast(AvatarSceneInfo avatar, PlayerInstance player)
    {
        await System.Threading.Tasks.Task.CompletedTask;
    }

    public async ValueTask OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        await System.Threading.Tasks.Task.CompletedTask;
    }
}