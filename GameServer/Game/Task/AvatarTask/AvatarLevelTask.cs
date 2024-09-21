using EggLink.DanhengServer.Data.Config.Task;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Task.AvatarTask;

public class AvatarLevelTask
{
    #region Task Condition

    public bool ByIsContainAdventureModifier(TaskConfigInfo act, List<IGameEntity> targetEntities,
        EntitySummonUnit? summonUnit)
    {
        return true;
    }

    #endregion

    #region Manage

    public void TriggerTasks(List<TaskConfigInfo> tasks, List<IGameEntity> targetEntities, EntitySummonUnit? summonUnit)
    {
        foreach (var task in tasks) TriggerTask(task, targetEntities, summonUnit);
    }

    public void TriggerTask(TaskConfigInfo act, List<IGameEntity> targetEntities, EntitySummonUnit? summonUnit)
    {
        try
        {
            var methodName = act.Type.Replace("RPG.GameCore.", "");

            var method = GetType().GetMethod(methodName);
            if (method != null) _ = method.Invoke(this, [act, targetEntities, summonUnit]);
        }
        catch
        {
        }
    }

    #endregion

    #region Task

    public async ValueTask PredicateTaskList(TaskConfigInfo act, List<IGameEntity> targetEntities,
        EntitySummonUnit? summonUnit)
    {
        if (act is PredicateTaskList predicateTaskList)
        {
            // handle predicateCondition
            var methodName = predicateTaskList.Predicate.Type.Replace("RPG.GameCore.", "");

            var method = GetType().GetMethod(methodName);
            if (method != null)
            {
                var resp = method.Invoke(this, [predicateTaskList.Predicate, targetEntities, summonUnit]);
                if (resp is bool res && res)
                    foreach (var task in predicateTaskList.SuccessTaskList)
                        TriggerTask(task, targetEntities, summonUnit);
                else
                    foreach (var task in predicateTaskList.FailedTaskList)
                        TriggerTask(task, targetEntities, summonUnit);
            }
        }

        await ValueTask.CompletedTask;
    }

    public async ValueTask AddMazeBuff(TaskConfigInfo act, List<IGameEntity> targetEntities,
        EntitySummonUnit? summonUnit)
    {
        if (act is not AddMazeBuff addMazeBuff) return;

        var buff = new SceneBuff(addMazeBuff.ID, 1, summonUnit?.CreateAvatarId ?? 0)
        {
            SummonUnitEntityId = summonUnit?.EntityID ?? 0
        };

        foreach (var item in addMazeBuff.DynamicValues)
            buff.DynamicValues.Add(item.Key, item.Value.GetValue());

        foreach (var targetEntity in targetEntities)
        {
            if (targetEntity is not EntityMonster monster) continue;

            await monster.AddBuff(buff);
        }
    }

    public async ValueTask RemoveMazeBuff(TaskConfigInfo act, List<IGameEntity> targetEntities,
        EntitySummonUnit? summonUnit)
    {
        if (act is not RemoveMazeBuff removeMazeBuff) return;

        foreach (var targetEntity in targetEntities)
        {
            if (targetEntity is not EntityMonster monster) continue;

            await monster.RemoveBuff(removeMazeBuff.ID);
        }
    }

    public async ValueTask RefreshMazeBuffTime(TaskConfigInfo act, List<IGameEntity> targetEntities,
        EntitySummonUnit? summonUnit)
    {
        if (act is not RefreshMazeBuffTime refreshMazeBuffTime) return;

        var buff = new SceneBuff(refreshMazeBuffTime.ID, 1, summonUnit?.CreateAvatarId ?? 0)
        {
            SummonUnitEntityId = summonUnit?.EntityID ?? 0,
            Duration = refreshMazeBuffTime.LifeTime.GetValue() == 0 ? -1 : refreshMazeBuffTime.LifeTime.GetValue()
        };

        foreach (var targetEntity in targetEntities)
        {
            if (targetEntity is not EntityMonster monster) continue;

            await monster.AddBuff(buff);
        }
    }

    public async ValueTask TriggerHitProp(TaskConfigInfo act, List<IGameEntity> targetEntities,
        EntitySummonUnit? summonUnit)
    {
        foreach (var targetEntity in targetEntities)
        {
            if (targetEntity is not EntityProp prop) continue;

            await prop.Scene.RemoveEntity(prop);
            if (prop.Excel.IsMpRecover)
            {
                await prop.Scene.Player.LineupManager!.GainMp(2, true, SyncLineupReason.SyncReasonMpAddPropHit);
            }
            else if (prop.Excel.IsHpRecover)
            {
                prop.Scene.Player.LineupManager!.GetCurLineup()!.Heal(2000, false);
                await prop.Scene.Player.SendPacket(
                    new PacketSyncLineupNotify(prop.Scene.Player.LineupManager!.GetCurLineup()!));
            }
            else
            {
                prop.Scene.Player.InventoryManager!.HandlePlaneEvent(prop.PropInfo.EventID);
            }

            prop.Scene.Player.RogueManager!.GetRogueInstance()?.OnPropDestruct(prop);
        }
    }

    #endregion
}