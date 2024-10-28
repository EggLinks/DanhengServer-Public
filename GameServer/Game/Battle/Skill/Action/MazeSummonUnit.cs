using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Battle.Skill.Action;

public class MazeSummonUnit(SummonUnitDataExcel excel, MotionInfo motion) : IMazeSkillAction
{
    public async ValueTask OnCast(AvatarSceneInfo avatar, PlayerInstance player)
    {
        var unit = new EntitySummonUnit
        {
            EntityID = 0,
            CreateAvatarEntityId = avatar.EntityID,
            AttachEntityId = excel.ConfigInfo?.AttachPoint == "Origin" ? avatar.EntityID : 0,
            SummonUnitId = excel.ID,
            CreateAvatarId = avatar.AvatarInfo.GetAvatarId(),
            LifeTimeMs = 20000,
            TriggerList = excel.ConfigInfo?.TriggerConfig.CustomTriggers ?? [],
            Motion = motion
        };

        await player.SceneInstance!.AddSummonUnitEntity(unit);
    }

    public async ValueTask OnHitTarget(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        await ValueTask.CompletedTask;
    }

    public async ValueTask OnAttack(AvatarSceneInfo avatar, List<EntityMonster> entities)
    {
        await ValueTask.CompletedTask;
    }
}