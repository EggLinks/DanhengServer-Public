using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.RogueMagic;
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
                var instance = entity.Scene.Player.RogueManager!.GetRogueInstance();
                switch (instance)
                {
                    case null:
                        continue;
                    case RogueMagicInstance magic:
                        await magic.RollMagicUnit(1, 1, [RogueMagicUnitCategoryEnum.Common]);
                        break;
                    default:
                        await instance.RollBuff(1);
                        break;
                }

                await instance.GainMoney(Random.Shared.Next(20, 60));
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