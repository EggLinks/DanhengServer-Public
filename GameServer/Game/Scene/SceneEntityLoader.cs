using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

namespace EggLink.DanhengServer.GameServer.Game.Scene;

public class SceneEntityLoader(SceneInstance scene)
{
    public SceneInstance Scene { get; set; } = scene;
    public List<int> LoadGroups { get; set; } = [];

    public virtual async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;

        var dimInfo = Scene.FloorInfo?.DimensionList.Find(x => x.ID == 0);
        if (dimInfo == null) return;
        LoadGroups.AddRange(dimInfo.GroupIDList);

        foreach (var group in from @group in Scene.FloorInfo?.Groups.Values!
                 where @group.LoadSide != GroupLoadSideEnum.Client
                 where !@group.GroupName.Contains("DeployPuzzle_Repeat_Area")
                 where !@group.GroupName.Contains("TrainVisitor")
                 where !@group.GroupName.Contains("TrainVisiter")
                 select @group) await LoadGroup(group);

        Scene.IsLoaded = true;
    }

    public virtual async ValueTask SyncEntity()
    {
        var refreshed = false;
        var oldGroupId = new List<int>();
        foreach (var entity in Scene.Entities.Values.Where(entity => !oldGroupId.Contains(entity.GroupID)))
            oldGroupId.Add(entity.GroupID);

        var removeList = new List<IGameEntity>();
        var addList = new List<IGameEntity>();

        foreach (var group in Scene.FloorInfo!.Groups.Values
                     .Where(group => group.LoadSide != GroupLoadSideEnum.Client)
                     .Where(group => !group.GroupName.Contains("TrainVisitor"))
                     .Where(group => !group.GroupName.Contains("DeployPuzzle_Repeat_Area"))
                     .Where(group => !group.GroupName.Contains("TrainVisiter")))

            if (oldGroupId.Contains(group.Id)) // check if it should be unloaded
            {
                if (group.ForceUnloadCondition.IsTrue(Scene.Player.MissionManager!.Data,
                        false) || // condition: Force Unload Condition
                    group.UnloadCondition.IsTrue(Scene.Player.MissionManager!.Data,
                        false)) // condition: Unload Condition  anyone of the conditions is true then unload
                {
                    foreach (var entity in Scene.Entities.Values.Where(entity => entity.GroupID == group.Id))
                    {
                        await Scene.RemoveEntity(entity, false);
                        removeList.Add(entity);
                        refreshed = true;
                    }

                    Scene.Groups.Remove(group.Id);
                }
                else if (group.OwnerMainMissionID != 0 &&
                         Scene.Player.MissionManager!.GetMainMissionStatus(group.OwnerMainMissionID) !=
                         MissionPhaseEnum.Accept) // condition: Owner Main Mission ID
                {
                    foreach (var entity in Scene.Entities.Values.Where(entity => entity.GroupID == group.Id))
                    {
                        await Scene.RemoveEntity(entity, false);
                        removeList.Add(entity);
                        refreshed = true;
                    }

                    Scene.Groups.Remove(group.Id);
                }
            }
            else // check if it should be loaded
            {
                var groupList = await LoadGroup(group);
                refreshed = groupList != null || refreshed;
                addList.AddRange(groupList ?? []);
            }

        if (refreshed && (addList.Count > 0 || removeList.Count > 0))
            await Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(addList, removeList));
    }

    public virtual async ValueTask<List<IGameEntity>?> LoadGroup(GroupInfo info, bool forceLoad = false)
    {
        if (!LoadGroups.Contains(info.Id)) return null; // check if group should be loaded in this dimension
        var missionData = Scene.Player.MissionManager!.Data; // get mission data
        if (info.LoadSide == GroupLoadSideEnum.Client) return null; // check if group should be loaded on client side
        if (info.GroupName.Contains("TrainVisitor")) return null; // certain group name
        if (info.GroupName.Contains("DeployPuzzle_Repeat_Area")) return null;
        if (info.GroupName.Contains("TrainVisiter")) return null;

        if (info.SystemUnlockCondition != null) // condition: System Unlock Condition
        {
            var result = info.SystemUnlockCondition.Operation != OperationEnum.Or; // operation
            foreach (var conditionId in info.SystemUnlockCondition.Conditions)
            {
                GameData.GroupSystemUnlockDataData.TryGetValue(conditionId, out var unlockExcel);
                if (unlockExcel == null) continue;
                var part = Scene.Player.QuestManager?.UnlockHandler.GetUnlockStatus(unlockExcel.UnlockID) ??
                           false; // check if unlock condition is met
                if (info.SystemUnlockCondition.Operation == OperationEnum.Or && part)
                {
                    result = true;
                    break;
                }

                if (info.SystemUnlockCondition.Operation == OperationEnum.And && !part)
                {
                    result = false;
                    break;
                }

                if (info.SystemUnlockCondition.Operation != OperationEnum.Not || !part) continue;
                result = false;
                break;
            }

            if (!result) return null;
        }

        if (!(info.OwnerMainMissionID == 0 || // condition: Owner Main Mission ID
              Scene.Player.MissionManager!.GetMainMissionStatus(info.OwnerMainMissionID) ==
              MissionPhaseEnum.Accept)) return null; // check if main mission is accepted

        if (Scene.FloorId == 20332001 && info.Id == 109) // certain group id
            if (Scene.Player.SceneData?.FloorSavedData.GetValueOrDefault(20332001, [])
                    .GetValueOrDefault("ShowFeather", 0) != 1)
                return null; // a temp solution for Sunday

        if ((!info.LoadCondition.IsTrue(missionData) ||
             info.UnloadCondition.IsTrue(missionData,
                 false) || // condition: Load Condition, Unload Condition, Force Unload Condition
             info.ForceUnloadCondition.IsTrue(missionData, false)) &&
            !forceLoad) return null; // check if group should be loaded forcefully

        if (!info.SavedValueCondition.IsTrue(
                Scene.Player.SceneData!.FloorSavedData.GetValueOrDefault(Scene.FloorId, [])) &&
            !forceLoad) // condition: Saved Value Condition
            return null;

        if (Scene.Entities.Values.ToList().FindIndex(x => x.GroupID == info.Id) !=
            -1) // check if group is already loaded
            return null;

        // load
        Scene.Groups.Add(info.Id); // add group to loaded groups

        var entityList = new List<IGameEntity>();
        foreach (var npc in info.NPCList)
            try
            {
                if (await LoadNpc(npc, info) is { } entity) entityList.Add(entity);
            }
            catch
            {
                // ignored
            }

        foreach (var monster in info.MonsterList)
            try
            {
                if (await LoadMonster(monster, info) is { } entity) entityList.Add(entity);
            }
            catch
            {
                // ignored
            }

        foreach (var prop in info.PropList)
            try
            {
                if (await LoadProp(prop, info) is { } entity) entityList.Add(entity);
            }
            catch
            {
                // ignored
            }

        return entityList;
    }

    public virtual async ValueTask<List<IGameEntity>?> LoadGroup(int groupId, bool sendPacket = true)
    {
        var group = Scene.FloorInfo?.Groups.TryGetValue(groupId, out var v1) == true ? v1 : null;
        if (group == null) return null;
        var entities = await LoadGroup(group, true);

        if (sendPacket && entities is { Count: > 0 })
            await Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(entities));

        return entities;
    }

    public virtual async ValueTask UnloadGroup(int groupId)
    {
        var group = Scene.FloorInfo?.Groups.TryGetValue(groupId, out var v1) == true ? v1 : null;
        if (group == null) return;

        var removeList = new List<IGameEntity>();
        var refreshed = false;

        foreach (var entity in Scene.Entities.Values)
            if (entity.GroupID == group.Id)
            {
                await Scene.RemoveEntity(entity, false);
                removeList.Add(entity);
                refreshed = true;
            }

        Scene.Groups.Remove(group.Id);

        if (refreshed) await Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(removeEntity: removeList));
    }

    public virtual async ValueTask<EntityNpc?> LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;

        if (!GameData.NpcDataData.ContainsKey(info.NPCID)) return null;

        EntityNpc npc = new(Scene, group, info);
        await Scene.AddEntity(npc, sendPacket);

        return npc;
    }

    public virtual async ValueTask<EntityMonster?> LoadMonster(MonsterInfo info, GroupInfo group,
        bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;

        GameData.NpcMonsterDataData.TryGetValue(info.NPCMonsterID, out var excel);
        if (excel == null) return null;

        EntityMonster entity = new(Scene, info.ToPositionProto(), info.ToRotationProto(), group.Id, info.ID, excel,
            info);
        await Scene.AddEntity(entity, sendPacket);
        return entity;
    }

    public virtual async ValueTask<EntityProp?> LoadProp(PropInfo info, GroupInfo group, bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete || !info.LoadOnInitial) return null;

        GameData.MazePropData.TryGetValue(info.PropID, out var excel);
        if (excel == null) return null;

        var prop = new EntityProp(Scene, excel, group, info);

        if (excel.PropType == PropTypeEnum.PROP_SPRING)
        {
            Scene.HealingSprings.Add(prop);
            await prop.SetState(PropStateEnum.CheckPointEnable);
        }

        // load from database
        var propData = Scene.Player.GetScenePropData(Scene.FloorId, group.Id, info.ID);
        if (propData != null && Scene.Excel.PlaneType != PlaneTypeEnum.Raid) // raid is not saved
        {
            prop.State = propData.State;
        }
        else
        {
            if (Scene.Excel.PlaneType == PlaneTypeEnum.Raid)
                prop.State = info.State;
            else
                // elevator
                prop.State = prop.Excel.PropType == PropTypeEnum.PROP_ELEVATOR ? PropStateEnum.Elevator1 : info.State;
        }

        if (group.GroupName.Contains("Machine"))
        {
            await prop.SetState(PropStateEnum.Open);
            await Scene.AddEntity(prop, sendPacket);
            return prop;
        }

        if (prop.PropInfo.Name.Contains("Case") && prop.PropInfo.State == PropStateEnum.Open)
            await prop.SetState(PropStateEnum.Closed);

        if (prop.PropInfo.PropID == 1003)
        {
            if (prop.PropInfo.MappingInfoID != 2220) return prop;
            await prop.SetState(PropStateEnum.Open);
            await Scene.AddEntity(prop, sendPacket);
        }
        else
        {
            await Scene.AddEntity(prop, sendPacket);
        }

        return prop;
    }
}