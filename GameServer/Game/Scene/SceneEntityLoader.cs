using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Game.Scene
{
    public class SceneEntityLoader(SceneInstance scene)
    {
        public SceneInstance Scene { get; set; } = scene;

        public virtual void LoadEntity()
        {
            if (Scene.IsLoaded) return;

            foreach (var group in Scene?.FloorInfo?.Groups.Values!)  // Sanity check in SceneInstance
            {
                if (group.LoadSide == GroupLoadSideEnum.Client)
                {
                    continue;
                }
                if (group.GroupName.Contains("TrainVisitor"))
                {
                    continue;
                }
                LoadGroup(group);
            }
            Scene.IsLoaded = true;
        }

        public void SyncEntity()
        {
            if (Scene.Excel.PlaneType == PlaneTypeEnum.Raid) return;

            bool refreshed = false;
            var oldGroupId = new List<int>();
            foreach (var entity in Scene.Entities.Values)
            {
                if (!oldGroupId.Contains(entity.GroupID))
                    oldGroupId.Add(entity.GroupID);
            }

            var removeList = new List<IGameEntity>();
            var addList = new List<IGameEntity>();

            foreach (var group in Scene.FloorInfo!.Groups.Values)
            {
                if (group.LoadSide == GroupLoadSideEnum.Client)
                {
                    continue;
                }

                if (group.GroupName.Contains("TrainVisitor"))
                {
                    continue;
                }

                if (oldGroupId.Contains(group.Id))  // check if it should be unloaded
                {
                    if (group.ForceUnloadCondition.IsTrue(Scene.Player.MissionManager!.Data, false) || group.UnloadCondition.IsTrue(Scene.Player.MissionManager!.Data, false))
                    {
                        foreach (var entity in Scene.Entities.Values)
                        {
                            if (entity.GroupID == group.Id)
                            {
                                Scene.RemoveEntity(entity, false);
                                removeList.Add(entity);
                                refreshed = true;
                            }
                        }
                        Scene.Groups.Remove(group.Id);
                    }
                    else if (group.OwnerMainMissionID != 0 && Scene.Player.MissionManager!.GetMainMissionStatus(group.OwnerMainMissionID) != Enums.MissionPhaseEnum.Accept)
                    {
                        foreach (var entity in Scene.Entities.Values)
                        {
                            if (entity.GroupID == group.Id)
                            {
                                Scene.RemoveEntity(entity, false);
                                removeList.Add(entity);
                                refreshed = true;
                            }
                        }
                        Scene.Groups.Remove(group.Id);
                    }
                } else  // check if it should be loaded
                {
                    var groupList = LoadGroup(group);
                    refreshed = groupList != null || refreshed;
                    addList.AddRange(groupList ?? []);
                }
            }
            if (refreshed && (addList.Count > 0 || removeList.Count > 0))
            {
                Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(addList, removeList));
            }
        }

        public virtual List<IGameEntity>? LoadGroup(GroupInfo info, bool forceLoad = false)
        {
            var missionData = Scene.Player.MissionManager!.Data;
            if (info.LoadSide == GroupLoadSideEnum.Client)
            {
                return null;
            }

            if (info.GroupName.Contains("TrainVisitor"))
            {
                return null;
            }

            if (Scene.Excel.PlaneType != PlaneTypeEnum.Raid)
            {
                if (!(info.OwnerMainMissionID == 0 || Scene.Player.MissionManager!.GetMainMissionStatus(info.OwnerMainMissionID) == Enums.MissionPhaseEnum.Accept))
                {
                    return null;
                }

                if ((!info.LoadCondition.IsTrue(missionData) || info.UnloadCondition.IsTrue(missionData, false) || info.ForceUnloadCondition.IsTrue(missionData, false)) && !forceLoad)
                {
                    return null;
                }
            }

            if (Scene.Entities.Values.ToList().FindIndex(x => x.GroupID == info.Id) != -1)  // check if group is already loaded
            {
                return null;
            }

            // load
            Scene.Groups.Add(info.Id);

            var entityList = new List<IGameEntity>();
            foreach (var npc in info.NPCList)
            {
                try
                {
                    if (LoadNpc(npc, info) is EntityNpc entity)
                    {
                        entityList.Add(entity);
                    }
                } catch{ }
            }

            foreach (var monster in info.MonsterList)
            {
                try
                {
                    if (LoadMonster(monster, info) is EntityMonster entity)
                    {
                        entityList.Add(entity);
                    }
                } 
                catch { }
            }

            foreach (var prop in info.PropList)
            {
                try
                {
                    if (LoadProp(prop, info) is EntityProp entity)
                    {
                        entityList.Add(entity);
                    }
                } catch { }
            }

            return entityList;
        }

        public virtual List<IGameEntity>? LoadGroup(int groupId, bool sendPacket = true)
        {
            var group = Scene.FloorInfo?.Groups.TryGetValue(groupId, out GroupInfo? v1) == true ? v1 : null;
            if (group == null) { return null; }
            var entities = LoadGroup(group, true);

            if (sendPacket && entities != null && entities.Count > 0)
            {
                Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(addEntity: entities));
            }

            return entities;
        }

        public virtual void UnloadGroup(int groupId)
        {
            var group = Scene.FloorInfo?.Groups.TryGetValue(groupId, out GroupInfo? v1) == true ? v1 : null;
            if (group == null) return;

            var removeList = new List<IGameEntity>();
            bool refreshed = false;

            foreach (var entity in Scene.Entities.Values)
            {
                if (entity.GroupID == group.Id)
                {
                    Scene.RemoveEntity(entity, false);
                    removeList.Add(entity);
                    refreshed = true;
                }
            }
            Scene.Groups.Remove(group.Id);

            if (refreshed)
            {
                Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(removeEntity:removeList));
            }
        }

        public virtual EntityNpc? LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
        {
            if (info.IsClientOnly || info.IsDelete)
            {
                return null;
            }

            if (group.Id == 117)
            {
                GameData.GetAvatarExpRequired(0, 0);
            }

            if (!GameData.NpcDataData.ContainsKey(info.NPCID))
            {
                return null;
            }

            bool hasDuplicateNpcId = false;
            foreach (IGameEntity entity in Scene.Entities.Values)
            {
                if (entity is EntityNpc eNpc && eNpc.NpcId == info.NPCID)
                {
                    hasDuplicateNpcId = true;
                    break;
                }
            }

            if (hasDuplicateNpcId)
            {
                //return null;
            }

            EntityNpc npc = new(Scene, group, info);
            Scene.AddEntity(npc, sendPacket);

            return npc;
        }

        public virtual EntityMonster? LoadMonster(MonsterInfo info, GroupInfo group, bool sendPacket = false)
        {
            if (info.IsClientOnly || info.IsDelete)
            {
                return null;
            }

            GameData.NpcMonsterDataData.TryGetValue(info.NPCMonsterID, out var excel);
            if (excel == null)
            {
                return null;
            }

            EntityMonster entity = new(Scene, info.ToPositionProto(), info.ToRotationProto(), group.Id, info.ID, excel, info);
            Scene.AddEntity(entity, sendPacket);
            return entity;
        }

        public virtual EntityProp? LoadProp(PropInfo info, GroupInfo group, bool sendPacket = false)
        {
            if (info.IsClientOnly || info.IsDelete)
            {
                return null;
            }

            GameData.MazePropData.TryGetValue(info.PropID, out var excel);
            if (excel == null)
            {
                return null;
            }

            var prop = new EntityProp(Scene, excel, group, info);

            if (excel.PropType == PropTypeEnum.PROP_SPRING)
            {
                Scene.HealingSprings.Add(prop);
                prop.SetState(PropStateEnum.CheckPointEnable);
            }

            // load from database
            var propData = Scene.Player.GetScenePropData(Scene.FloorId, group.Id, info.ID);
            if (propData != null && Scene.Excel.PlaneType != PlaneTypeEnum.Raid)  // raid is not saved
            {
                prop.State = propData.State;
            } 
            else
            {
                if (Scene.Excel.PlaneType == PlaneTypeEnum.Raid)
                {
                    prop.State = info.State;
                } 
                else
                {
                    // elevator
                    if (prop.Excel.PropType == PropTypeEnum.PROP_ELEVATOR)
                    {
                        prop.State = PropStateEnum.Elevator1;
                    } 
                    else
                    {
                        prop.State = info.State;
                    }
                }
            }

            if (prop.PropInfo.PropID == 1003)
            {
                if (prop.PropInfo.MappingInfoID == 2220)
                {
                    prop.SetState(PropStateEnum.Open);
                    Scene.AddEntity(prop, sendPacket);
                }
            }
            else
            {
                Scene.AddEntity(prop, sendPacket);
            }

            return prop;
        }
    }
}
