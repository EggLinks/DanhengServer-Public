using EggLink.DanhengServer.Data.Config.Task;
using EggLink.DanhengServer.Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Task;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Task
{
    public class LevelTask(PlayerInstance player)
    {
        public PlayerInstance Player { get; } = player;

        #region Manage

        public void TriggerInitAct(LevelInitSequeceConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            foreach (var task in act.TaskList)
            {
                TriggerTask(task, subMission, group);
            }
        }

        public void TriggerStartAct(LevelStartSequeceConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            foreach (var task in act.TaskList)
            {
                TriggerTask(task, subMission, group);
            }
        }

        private void TriggerTask(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            try
            {
                var methodName = act.Type.Replace("RPG.GameCore.", "");

                var method = GetType().GetMethod(methodName);
                if (method != null)
                {
                    _ = method.Invoke(this, [act, subMission, group]);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Task

        public void PlayMessage(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is PlayMessage message)
            {
                Player.MessageManager!.AddMessageSection(message.MessageSectionID);
            }
        }

        public void DestroyProp(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is DestroyProp destroyProp)
            {
                foreach (var entity in Player.SceneInstance!.Entities.Values)
                {
                    if (entity is EntityProp prop && prop.GroupID == destroyProp.GroupID.GetValue() && prop.InstId == destroyProp.ID.GetValue())
                    {
                        Player.SceneInstance.RemoveEntity(entity);
                    }
                }
            }
        }

        public void TriggerCustomString(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is TriggerCustomString triggerCustomString)
            {
                foreach (var groupInfo in Player.SceneInstance?.FloorInfo?.Groups ?? [])
                {
                    if (groupInfo.Value.PropTriggerCustomString.TryGetValue(triggerCustomString.CustomString.Value, out var list))
                    {
                        foreach (var id in list)
                        {
                            foreach (var entity in Player.SceneInstance?.Entities.Values.ToList() ?? [])
                            {
                                if (entity is EntityProp prop && prop.GroupID == groupInfo.Key && prop.InstId == id)
                                {
                                    prop.SetState(PropStateEnum.Closed);
                                }
                            }
                        }
                    }
                }

                Player.MissionManager?.HandleFinishType(MissionFinishTypeEnum.PropState);
            }
        }

        public void EnterMap(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is EnterMap enterMap)
            {
                Player.EnterMissionScene(enterMap.EntranceID, enterMap.GroupID, enterMap.AnchorID, true);
            }
        }

        public void EnterMapByCondition(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is EnterMapByCondition enterMapByCondition)
            {
                Player.EnterMissionScene(enterMapByCondition.EntranceID.GetValue(), 0, 0, true);
            }
        }

        public void TriggerPerformance(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is TriggerPerformance triggerPerformance)
            {
                if (triggerPerformance.PerformanceType == ELevelPerformanceTypeEnum.E)
                {
                    Player.TaskManager?.PerformanceTrigger.TriggerPerformanceE(triggerPerformance.PerformanceID, subMission);
                }
                else if (triggerPerformance.PerformanceType == ELevelPerformanceTypeEnum.D)
                {
                    Player.TaskManager?.PerformanceTrigger.TriggerPerformanceD(triggerPerformance.PerformanceID, subMission);
                }
            }
        }

        public void PredicateTaskList(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is PredicateTaskList predicateTaskList)
            {
                // handle predicateCondition
                var methodName = predicateTaskList.Predicate.Type.Replace("RPG.GameCore.", "");

                var method = GetType().GetMethod(methodName);
                if (method != null)
                {
                    var resp = method.Invoke(this, [predicateTaskList.Predicate, subMission, group]);
                    if (resp is bool result && result)
                    {
                        foreach (var task in predicateTaskList.SuccessTaskList)
                        {
                            TriggerTask(task, subMission, group);
                        }
                    } 
                    else
                    {
                        foreach (var task in predicateTaskList.FailedTaskList)
                        {
                            TriggerTask(task, subMission, group);
                        }
                    }
                }
            }
        }

        public void ChangePropState(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.PropState)
            {
                foreach (var entity in Player.SceneInstance!.Entities.Values)
                {
                    if (entity is EntityProp prop && prop.GroupID == subMission.SubMissionInfo.ParamInt1 && prop.InstId == subMission.SubMissionInfo.ParamInt2)
                    {
                        try
                        {
                            if (prop.Excel.PropStateList.Contains(PropStateEnum.Closed))
                            {
                                prop.SetState(PropStateEnum.Closed);
                            }
                            else
                            {
                                prop.SetState(prop.Excel.PropStateList[prop.Excel.PropStateList.IndexOf(prop.State) + 1]);

                                // Elevator
                                foreach (var id in prop.PropInfo.UnlockControllerID)
                                {
                                    foreach (var entity2 in Player.SceneInstance!.Entities.Values)
                                    {
                                        if (entity2 is EntityProp prop2 && prop2.GroupID == id.Key && id.Value.Contains(prop2.InstId))
                                        {
                                            prop2.SetState(PropStateEnum.Closed);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public void CreateTrialPlayer(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatar)
            {
                Player.LineupManager!.AddAvatarToCurTeam(subMission.SubMissionInfo.ParamInt1);
            }

            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatarList)
            {
                subMission.SubMissionInfo.ParamIntList?.ForEach(x => Player.LineupManager!.AddAvatarToCurTeam(x));
            }
        }

        public void ReplaceTrialPlayer(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatar)
            {
                var ids = Player.LineupManager!.GetCurLineup()?.BaseAvatars?.ToList() ?? [];
                ids.ForEach(x => Player.LineupManager!.RemoveAvatarFromCurTeam(x.BaseAvatarId, false));
                Player.LineupManager!.AddAvatarToCurTeam(subMission.SubMissionInfo.ParamInt1);
            }

            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatarList)
            {
                var ids = Player.LineupManager!.GetCurLineup()?.BaseAvatars?.ToList() ?? [];
                ids.ForEach(x => Player.LineupManager!.RemoveAvatarFromCurTeam(x.BaseAvatarId, false));
                subMission.SubMissionInfo.ParamIntList?.ForEach(x => Player.LineupManager!.AddAvatarToCurTeam(x));
            }
        }

        public void ReplaceVirtualTeam(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (!(Player.LineupManager!.GetCurLineup()?.IsExtraLineup() == true))
            {
                return;
            }

            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatar)
            {
                var ids = Player.LineupManager!.GetCurLineup()?.BaseAvatars?.ToList() ?? [];
                ids.ForEach(x => Player.LineupManager!.RemoveAvatarFromCurTeam(x.BaseAvatarId, false)); ;
                Player.LineupManager!.AddAvatarToCurTeam(subMission.SubMissionInfo.ParamInt1);
            }

            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatarList)
            {
                var ids = Player.LineupManager!.GetCurLineup()?.BaseAvatars?.ToList() ?? [];
                ids.ForEach(x => Player.LineupManager!.RemoveAvatarFromCurTeam(x.BaseAvatarId, false));
                subMission.SubMissionInfo.ParamIntList?.ForEach(x => Player.LineupManager!.AddAvatarToCurTeam(x));
            }
        }

        public void CreateHeroTrialPlayer(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatar)
            {
                Player.LineupManager!.AddAvatarToCurTeam(subMission.SubMissionInfo.ParamInt1);
            }

            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.GetTrialAvatarList)
            {
                List<int> list = [.. subMission.SubMissionInfo.ParamIntList];

                if (list.Count > 0)
                {
                    if (Player.Data.CurrentGender == Gender.Man)
                    {
                        foreach (var avatar in subMission.SubMissionInfo?.ParamIntList ?? [])
                        {
                            if (avatar > 10000)  // else is Base Avatar
                            {
                                if (avatar.ToString().EndsWith("8002") ||
                                    avatar.ToString().EndsWith("8004") ||
                                    avatar.ToString().EndsWith("8006"))
                                {
                                    list.Remove(avatar);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var avatar in subMission.SubMissionInfo?.ParamIntList ?? [])
                        {
                            if (avatar > 10000)  // else is Base Avatar
                            {
                                if (avatar.ToString().EndsWith("8001") ||
                                    avatar.ToString().EndsWith("8003") ||
                                    avatar.ToString().EndsWith("8005"))
                                {
                                    list.Remove(avatar);
                                }
                            }
                        }
                    }
                }

                list.ForEach(x => Player.LineupManager!.AddAvatarToCurTeam(x));
            }
        }

        public void DestroyTrialPlayer(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (subMission.SubMissionInfo?.FinishType == MissionFinishTypeEnum.DelTrialAvatar)
            {
                Player.LineupManager!.RemoveAvatarFromCurTeam(subMission.SubMissionInfo.ParamInt1);
            }
        }

        public void ChangeGroupState(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (group != null)
            {
                foreach (var entity in Player.SceneInstance?.Entities.Values.ToList() ?? [])
                {
                    if (entity is EntityProp prop && prop.GroupID == group.Id)
                    {
                        if (prop.Excel.PropStateList.Contains(PropStateEnum.Open))
                        {
                            prop.SetState(PropStateEnum.Open);
                        }
                    }
                }
            }
        }

        public void TriggerEntityServerEvent(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (group != null)
            {
                foreach (var entity in Player.SceneInstance?.Entities.Values.ToList() ?? [])
                {
                    if (entity is EntityProp prop && prop.GroupID == group.Id)
                    {
                        if (prop.Excel.PropStateList.Contains(PropStateEnum.Open) && (prop.State == PropStateEnum.Closed || prop.State == PropStateEnum.Locked))
                        {
                            prop.SetState(PropStateEnum.Open);
                        }
                    }
                }
            }
        }

        public void TriggerEntityEvent(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is TriggerEntityEvent triggerEntityEvent)
            {
                if (group != null)
                {
                    foreach (var entity in Player.SceneInstance?.Entities.Values.ToList() ?? [])
                    {
                        if (entity is EntityProp prop && prop.GroupID == group.Id && prop.InstId == triggerEntityEvent.InstanceID.GetValue())
                        {
                            if (prop.Excel.PropStateList.Contains(PropStateEnum.Closed))
                            {
                                prop.SetState(PropStateEnum.Closed);
                            }
                        }
                    }
                }
            }
        }

        public void PropSetupUITrigger(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is PropSetupUITrigger propSetupUITrigger)
            {
                foreach (var task in propSetupUITrigger.ButtonCallback)
                {
                    TriggerTask(task, subMission, group);
                }
            }
        }

        public void PropStateExecute(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is PropStateExecute propStateExecute)
            {
                // handle targetType
                var methodName = propStateExecute.TargetType.Type.Replace("RPG.GameCore.", "");

                var method = GetType().GetMethod(methodName);
                if (method != null)
                {
                    var resp = method.Invoke(this, [propStateExecute.TargetType, subMission, group]);
                    if (resp is EntityProp result && result != null)
                    {
                        result.SetState(propStateExecute.State);
                    }
                }
            }
        }

        #endregion

        #region Task Condition

        public bool ByCompareSubMissionState(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is ByCompareSubMissionState compare)
            {
                var mission = Player.MissionManager!.GetSubMissionStatus(compare.SubMissionID);
                return mission.ToStateEnum() == compare.SubMissionState;
            }

            return false;
        }

        public bool ByCompareFloorSavedValue(TaskConfigInfo act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is ByCompareFloorSavedValue compare)
            {
                var value = Player.SceneData!.FloorSavedData.GetValueOrDefault(Player.Data.FloorId, []);
                return compare.CompareType switch
                {
                    CompareTypeEnum.Equal => value.GetValueOrDefault(compare.Name, 0) == compare.CompareValue,
                    CompareTypeEnum.Greater => value.GetValueOrDefault(compare.Name, 0) > compare.CompareValue,
                    CompareTypeEnum.Less => value.GetValueOrDefault(compare.Name, 0) < compare.CompareValue,
                    CompareTypeEnum.GreaterEqual => value.GetValueOrDefault(compare.Name, 0) >= compare.CompareValue,
                    CompareTypeEnum.LessEqual => value.GetValueOrDefault(compare.Name, 0) <= compare.CompareValue,
                    CompareTypeEnum.NotEqual => value.GetValueOrDefault(compare.Name, 0) != compare.CompareValue,
                    _ => false,
                };
            }

            return false;
        }

        #endregion

        #region Prop Target

        public EntityProp? TargetFetchAdvPropEx(TargetEvaluator act, SubMissionExcel subMission, GroupInfo? group = null)
        {
            if (act is TargetFetchAdvPropEx fetch)
            {
                if (fetch.FetchType != TargetFetchAdvPropFetchTypeEnum.SinglePropByPropID) return null;
                foreach (var entity in Player.SceneInstance!.Entities.Values)
                {
                    if (entity is EntityProp prop && prop.GroupID == fetch.SinglePropID.GroupID.GetValue() && prop.InstId == fetch.SinglePropID.ID.GetValue())
                    {
                        return prop;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
