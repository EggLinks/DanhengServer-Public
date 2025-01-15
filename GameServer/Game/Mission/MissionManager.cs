using System.Reflection;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums.Item;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Mission.FinishAction;
using EggLink.DanhengServer.GameServer.Game.Mission.FinishType;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Plugin.Event;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using MissionData = EggLink.DanhengServer.Database.Quests.MissionData;

namespace EggLink.DanhengServer.GameServer.Game.Mission;

public class MissionManager : BasePlayerManager
{
    #region Initializer & Properties

    public MissionData Data { get; set; }
    public Dictionary<FinishActionTypeEnum, MissionFinishActionHandler> ActionHandlers = [];
    public Dictionary<MissionFinishTypeEnum, MissionFinishTypeHandler> FinishTypeHandlers = [];

    public readonly List<int> SkipSubMissionList = []; // bug

    public MissionManager(PlayerInstance player) : base(player)
    {
        Data = DatabaseHelper.Instance!.GetInstanceOrCreateNew<MissionData>(player.Uid);

        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<MissionFinishActionAttribute>();
            if (attr != null)
            {
                var handler = (MissionFinishActionHandler)Activator.CreateInstance(type, null)!;
                ActionHandlers.Add(attr.FinishAction, handler);
            }

            var attr2 = type.GetCustomAttribute<MissionFinishTypeAttribute>();
            if (attr2 != null)
            {
                var handler = (MissionFinishTypeHandler)Activator.CreateInstance(type, null)!;
                FinishTypeHandlers.Add(attr2.FinishType, handler);
            }
        }
    }

    #endregion

    #region Mission Actions

    public async ValueTask<List<MissionSync?>> AcceptMainMission(int missionId, bool sendPacket = true)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return [];
        if (Data.GetMainMissionStatus(missionId) != MissionPhaseEnum.None) return []; // already accepted
        // Get entry sub mission
        GameData.MainMissionData.TryGetValue(missionId, out var mission);
        if (mission == null) return [];

        Data.SetMainMissionStatus(missionId, MissionPhaseEnum.Accept);

        var list = new List<MissionSync?>();
        mission.MissionInfo?.StartSubMissionList.ForEach(async i => list.Add(await AcceptSubMission(i, sendPacket)));
        if (missionId == 4030001 || missionId == 4030002)
        {
            // skip  not implemented
            mission.MissionInfo?.SubMissionList.ForEach(async x => await AcceptSubMission(x.ID));
            mission.MissionInfo?.SubMissionList.ForEach(async x => await FinishSubMission(x.ID));
        }

        if (missionId == 1000400)
        {
            await Player.AddAvatar(1003);
            await Player.LineupManager!.AddAvatarToCurTeam(1003);
        }

        // message
        foreach (var sectionConfigExcel in GameData.MessageSectionConfigData.Values.Where(x =>
                     x.MainMissionLink == missionId))
            await Player.MessageManager!.AddMessageSection(sectionConfigExcel.ID);

        foreach (var info in mission.MissionInfo!.SubMissionList.Where(x =>
                     x.FinishType is MissionFinishTypeEnum.MessagePerformSectionFinish
                         or MissionFinishTypeEnum.MessageSectionFinish))
            await Player.MessageManager!.AddMessageSection(info.ParamInt1);

        return list;
    }

    public async ValueTask<MissionSync> AcceptMainMissionByCondition(bool sendPacket = true)
    {
        var sync = new MissionSync();
        foreach (var nextMission in GameData.MainMissionData.Values)
        {
            if (!nextMission.IsEqual(Data)) continue;
            if (Data.GetMainMissionStatus(nextMission.MainMissionID) != MissionPhaseEnum.None)
                continue; // already accepted
            var res = await AcceptMainMission(nextMission.MainMissionID, sendPacket);
            foreach (var subMission in res)
                if (subMission != null)
                    sync.MissionList.AddRange(subMission.MissionList);
        }

        return sync;
    }

    public async ValueTask<List<MissionSync?>> ReAcceptMainMission(int missionId, bool sendPacket = true)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return [];

        GameData.MainMissionData.TryGetValue(missionId, out var mission);
        if (mission == null) return [];
        MissionSync sync = new();

        foreach (var subMission in mission.SubMissionIds)
            if (Data.GetSubMissionStatus(subMission) == MissionPhaseEnum.Finish ||
                Data.GetSubMissionStatus(subMission) == MissionPhaseEnum.Accept)
                sync.MissionList.Add(new Proto.Mission
                {
                    Id = (uint)subMission,
                    Status = MissionStatus.MissionNone
                });

        foreach (var subMission in
                 mission.SubMissionIds) Data.SetSubMissionStatus(subMission, MissionPhaseEnum.None); // reset

        Data.SetMainMissionStatus(missionId, MissionPhaseEnum.None); // reset
        await Player.SendPacket(new PacketPlayerSyncScNotify(sync));

        return await AcceptMainMission(missionId, sendPacket);
    }

    public async ValueTask RemoveMainMission(int missionId)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;
        Data.SetMainMissionStatus(missionId, MissionPhaseEnum.None);

        GameData.MainMissionData.TryGetValue(missionId, out var mission);
        if (mission == null) return;

        MissionSync sync = new();

        foreach (var subMission in mission.SubMissionIds)
        {
            Data.SetSubMissionStatus(subMission, MissionPhaseEnum.None);
            await SetMissionProgress(subMission, 0);
            sync.MissionList.Add(new Proto.Mission
            {
                Id = (uint)subMission,
                Status = MissionStatus.MissionNone
            });
        }

        await Player.SendPacket(new PacketPlayerSyncScNotify(sync));
    }

    public async ValueTask AcceptSubMission(int missionId)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;
        await AcceptSubMission(missionId, true);
    }

    public async ValueTask<MissionSync?> AcceptSubMission(int missionId, bool sendPacket,
        bool doFinishTypeAction = true)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return null;
        GameData.SubMissionData.TryGetValue(missionId, out var mission);
        if (mission == null) return null;
        if (Data.GetSubMissionStatus(missionId) != MissionPhaseEnum.None) return null; // already accepted

        Data.SetSubMissionStatus(missionId, MissionPhaseEnum.Accept);

        var sync = new MissionSync();
        sync.MissionList.Add(new Proto.Mission
        {
            Id = (uint)missionId,
            Status = MissionStatus.MissionDoing
        });

        if (sendPacket) await Player.SendPacket(new PacketPlayerSyncScNotify(sync));
        Player.SceneInstance!.SyncGroupInfo();
        if (mission.SubMissionInfo != null)
            try
            {
                FinishTypeHandlers.TryGetValue(mission.SubMissionInfo.FinishType, out var handler);
                if (doFinishTypeAction)
                    if (handler != null)
                        await handler.HandleMissionFinishType(Player, mission.SubMissionInfo, null);
            }
            catch
            {
            }

        if (SkipSubMissionList.Contains(missionId)) await FinishSubMission(missionId);

        if (mission.SubMissionInfo?.LevelFloorID == Player.SceneInstance?.FloorId)
            if (mission.SubMissionInfo?.GroupIDList != null)
                foreach (var group in mission.SubMissionInfo.GroupIDList)
                    await Player.SceneInstance!.EntityLoader!.LoadGroup(group);

        // TODO: Mission Task
        Player.TaskManager?.MissionTaskTrigger.TriggerMissionTask(missionId);

        return sync;
    }

    public async ValueTask FinishMainMission(int missionId)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;
        if (!GameData.MainMissionData.TryGetValue(missionId, out var mainMission)) return;
        if (Data.GetMainMissionStatus(missionId) != MissionPhaseEnum.Accept) return;
        Data.SetMainMissionStatus(missionId, MissionPhaseEnum.Finish);
        var sync = new MissionSync();
        sync.FinishedMainMissionIdList.Add((uint)missionId);
        // get next main mission
        foreach (var mission in mainMission.SubMissionIds)
            if (GetSubMissionStatus(mission) != MissionPhaseEnum.Finish)
                if (Data.GetSubMissionStatus(mission) != MissionPhaseEnum.Finish)
                {
                    Data.SetSubMissionStatus(mission, MissionPhaseEnum.Finish);
                    sync.MissionList.Add(new Proto.Mission
                    {
                        Id = (uint)mission,
                        Status = MissionStatus.MissionFinish
                    });
                }

        var mainSync = await AcceptMainMissionByCondition(false);
        sync.MissionList.AddRange(mainSync.MissionList);

        await Player.SendPacket(new PacketPlayerSyncScNotify(sync));
        await Player.SendPacket(new PacketStartFinishMainMissionScNotify(missionId));
        await HandleMissionReward(missionId);
        await HandleFinishType(MissionFinishTypeEnum.FinishMission);

        await Player.RaidManager!.CheckIfLeaveRaid();

        PluginEvent.InvokeOnPlayerFinishMainMission(Player, missionId);
    }

    public async ValueTask FinishSubMission(int missionId)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;
        GameData.SubMissionData.TryGetValue(missionId, out var subMission);
        if (subMission == null) return;
        var mainMissionId = subMission.MainMissionID;
        if (Data.GetSubMissionStatus(missionId) != MissionPhaseEnum.Accept) return; // not accepted
        GameData.MainMissionData.TryGetValue(mainMissionId, out var mainMission); // get main mission
        if (mainMission == null) return;
        Data.SetSubMissionStatus(missionId, MissionPhaseEnum.Finish); // set finish

        await SetMissionProgress(missionId, subMission.SubMissionInfo?.Progress ?? 1);

        var sync = new MissionSync();
        sync.MissionList.Add(new Proto.Mission
        {
            Id = (uint)missionId,
            Status = MissionStatus.MissionFinish,
            Progress = (uint)(subMission.SubMissionInfo?.Progress ?? 1)
        });

        // get next sub mission
        foreach (var nextMission in mainMission.MissionInfo?.SubMissionList ?? [])
        {
            if (nextMission.TakeType != SubMissionTakeTypeEnum.AnySequence &&
                nextMission.TakeType != SubMissionTakeTypeEnum.MultiSequence) continue;
            var canAccept = nextMission.TakeType == SubMissionTakeTypeEnum.MultiSequence; // mean and operation
            foreach (var id in nextMission.TakeParamIntList ?? [])
                if (GetSubMissionStatus(id) != MissionPhaseEnum.Finish &&
                    nextMission.TakeType == SubMissionTakeTypeEnum.MultiSequence)
                {
                    canAccept = false;
                    break;
                }
                else if (GetSubMissionStatus(id) == MissionPhaseEnum.Finish &&
                         nextMission.TakeType == SubMissionTakeTypeEnum.AnySequence) // or operation
                {
                    canAccept = true;
                    break;
                }

            if (canAccept)
            {
                var s = await AcceptSubMission(nextMission.ID, false, false);
                if (s != null)
                    sync.MissionList.Add(new Proto.Mission
                    {
                        Id = (uint)nextMission.ID,
                        Status = MissionStatus.MissionDoing
                    });
            }
        }

        await Player.SendPacket(new PacketPlayerSyncScNotify(sync));
        await Player.SendPacket(new PacketStartFinishSubMissionScNotify(missionId));

        if (mainMission.MissionInfo != null)
            await HandleFinishAction(mainMission.MissionInfo, missionId);

        // Get if it should finish main mission
        // get current main mission
        var shouldFinish = true;
        mainMission.MissionInfo?.FinishSubMissionList.ForEach(id =>
        {
            if (GetSubMissionStatus(id) != MissionPhaseEnum.Finish) shouldFinish = false;
        });

        foreach (var nextMission in GetRunningSubMissionList())
        {
            FinishTypeHandlers.TryGetValue(nextMission.FinishType, out var handler);
            if (handler != null) await handler.HandleMissionFinishType(Player, nextMission, null);
        }

        if (shouldFinish) await FinishMainMission(mainMissionId);

        if (missionId == 101140201)
        {
            // Player.ChangeAvatarPathType(8001, Enums.Avatar.MultiPathAvatarTypeEnum.Knight);
            var list = Player.LineupManager!.GetCurLineup()!.BaseAvatars!
                .Select(x => x.SpecialAvatarId > 0 ? x.SpecialAvatarId / 10 : x.BaseAvatarId).ToList();
            list[list.IndexOf(8001)] = Player.Data.CurrentGender == Gender.Man ? 1008003 : 1008004;
            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, list);
        }

        if (missionId == 103040103)
            await Player.SendPacket(new PacketHeartDialScriptChangeScNotify(HeartDialUnlockStatus.UnlockSingle));

        if (missionId == 103040104)
            await Player.SendPacket(new PacketHeartDialScriptChangeScNotify(HeartDialUnlockStatus.UnlockAll));

        // handle reward
        await HandleSubMissionReward(missionId);
        //Player.StoryLineManager!.CheckIfEnterStoryLine();
        //Player.StoryLineManager!.CheckIfFinishStoryLine();

        PluginEvent.InvokeOnPlayerFinishSubMission(Player, missionId);
    }

    public async ValueTask HandleFinishAction(MissionInfo info, int subMissionId)
    {
        var subMission = info.SubMissionList.Find(x => x.ID == subMissionId);
        if (subMission == null) return;

        foreach (var action in subMission.FinishActionList ?? []) await HandleFinishAction(action);
    }

    public async ValueTask HandleFinishAction(FinishActionInfo actionInfo)
    {
        ActionHandlers.TryGetValue(actionInfo.FinishActionType, out var handler);
        if (handler != null)
            await handler.OnHandle(actionInfo.FinishActionPara, actionInfo.FinishActionParaString, Player);
    }

    public async ValueTask HandleMissionReward(int mainMissionId)
    {
        GameData.MainMissionData.TryGetValue(mainMissionId, out var mainMission);
        if (mainMission == null) return;
        GameData.RewardDataData.TryGetValue(mainMission.RewardID, out var reward);
        var itemList = new List<ItemData>();

        foreach (var item in reward?.GetItems() ?? [])
        {
            GameData.ItemConfigData.TryGetValue(item.Item1, out var itemExcel);
            var res = await Player.InventoryManager!.AddItem(item.Item1, item.Item2,
                itemExcel?.ItemMainType == ItemMainTypeEnum.AvatarCard); // notify if avatar card
            if (res != null) itemList.Add(res);
        }

        var hCoin = await Player.InventoryManager!.AddItem(1, reward?.Hcoin ?? 0, false);
        if (hCoin != null) itemList.Add(hCoin);

        foreach (var i in mainMission.SubRewardList)
        {
            GameData.RewardDataData.TryGetValue(i, out var rewardDataExcel);
            var hCoin2 = await Player.InventoryManager!.AddItem(1, rewardDataExcel?.Hcoin ?? 0, false); // hcoin
            if (hCoin2 != null) itemList.Add(hCoin2);
            foreach (var item in rewardDataExcel?.GetItems() ?? []) // items
            {
                GameData.ItemConfigData.TryGetValue(item.Item1, out var itemExcel);
                var res = await Player.InventoryManager!.AddItem(item.Item1, item.Item2,
                    itemExcel?.ItemMainType == ItemMainTypeEnum.AvatarCard); // notify if avatar card
                if (res != null) itemList.Add(res);
            }
        }


        if (itemList.Count > 0)
            await Player.SendPacket(new PacketMissionRewardScNotify(mainMissionId, 0, itemList));
    }

    public async ValueTask HandleSubMissionReward(int subMissionId)
    {
        GameData.SubMissionData.TryGetValue(subMissionId, out var subMission);
        if (subMission == null) return;
        GameData.RewardDataData.TryGetValue(subMission.SubMissionInfo?.SubRewardID ?? 0, out var reward);
        var itemList = new List<ItemData>();

        foreach (var item in reward?.GetItems() ?? [])
        {
            GameData.ItemConfigData.TryGetValue(item.Item1, out var itemExcel);
            var res = await Player.InventoryManager!.AddItem(item.Item1, item.Item2,
                itemExcel?.ItemMainType == ItemMainTypeEnum.AvatarCard); // notify if avatar card
            if (res != null) itemList.Add(res);
        }

        await Player.SendPacket(new PacketSubMissionRewardScNotify(subMissionId, itemList));
    }

    public async ValueTask HandleFinishType(MissionFinishTypeEnum finishType, object? arg = null, bool pushQuest = true)
    {
        FinishTypeHandlers.TryGetValue(finishType, out var handler);
        foreach (var mission in GetRunningSubMissionList())
            if (mission.FinishType == finishType)
                if (handler != null)
                    await handler.HandleMissionFinishType(Player, mission, arg);

        foreach (var quest in Player.QuestManager?.GetRunningQuest() ?? [])
        {
            var excel = GameData.QuestDataData.GetValueOrDefault(quest.QuestId);
            if (excel == null) continue;
            var finishWay = GameData.FinishWayData.GetValueOrDefault(excel.FinishWayID);
            if (finishWay == null) continue;
            if (finishWay.FinishType == finishType)
                if (handler != null)
                    await handler.HandleQuestFinishType(Player, excel, finishWay, arg);
        }

        if (pushQuest)
            await Player.QuestManager!.SyncQuest();
    }

    public async ValueTask HandleAllFinishType(object? arg = null)
    {
        foreach (var handler in FinishTypeHandlers) await HandleFinishType(handler.Key, arg, false);
        await Player.QuestManager!.SyncQuest();
    }

    public async ValueTask HandleTalkStr(string talkString)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;

        foreach (var mission in GetRunningSubMissionList())
            if (mission.FinishType == MissionFinishTypeEnum.Talk)
                if (mission.ParamStr1 == talkString)
                    await FinishSubMission(mission.ID);

        foreach (var quest in Player.QuestManager?.GetRunningQuest() ?? [])
        {
            var excel = GameData.QuestDataData.GetValueOrDefault(quest.QuestId);
            if (excel == null) continue;
            var finishWay = GameData.FinishWayData.GetValueOrDefault(excel.FinishWayID);
            if (finishWay == null) continue;
            if (finishWay.FinishType == MissionFinishTypeEnum.Talk)
                if (finishWay.ParamStr1 == talkString)
                    await Player.QuestManager!.FinishQuest(quest.QuestId);
        }
    }

    public async ValueTask HandleCustomValue(List<MissionCustomValue> values, int missionId)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;

        GameData.SubMissionData.TryGetValue(missionId, out var subMission);
        if (subMission == null) return;
        var mainMissionId = subMission.MainMissionID;
        GameData.MainMissionData.TryGetValue(mainMissionId, out var mainMission);
        if (mainMission == null) return;

        foreach (var mission in mainMission.MissionInfo?.SubMissionList ?? [])
            if (mission.TakeType == SubMissionTakeTypeEnum.CustomValue)
            {
                var index = 0;
                var accept = false;
                List<List<int>> list = [mission.TakeParamIntList ?? []];
                if (mission.TakeParamIntList?.Count > 5)
                {
                    // every 3 as group
                    var group = mission.TakeParamIntList.Count / 3;
                    list = [];
                    for (var i = 0; i < group; i++)
                    {
                        var customValue = mission.TakeParamIntList.GetRange(i * 3, 3);
                        list.Add(customValue);
                    }
                }

                foreach (var customValues in list)
                {
                    var thisAccept = true;
                    foreach (var customValue in customValues)
                    {
                        if (customValue == 0 && index == 0) continue; // skip 0
                        var valueInst = values.Find(x => x.Index == index);
                        if (valueInst == null) continue;
                        if (valueInst.CustomValue != customValue)
                        {
                            thisAccept = false;
                            break;
                        }

                        index++;
                    }

                    if (thisAccept) accept = true; // accept if any group is true
                }

                if (accept) await AcceptSubMission(mission.ID);
            }
    }

    public async ValueTask AddMissionProgress(int missionId, int progress)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;

        Data.SubMissionProgressDict.TryGetValue(missionId, out var currentProgress);
        Data.SubMissionProgressDict[missionId] = currentProgress + progress;
        GameData.SubMissionData.TryGetValue(missionId, out var subMission);
        if (subMission == null) return;

        if (currentProgress + progress >= (subMission.SubMissionInfo?.Progress ?? 1)) return;

        var sync = new MissionSync();
        sync.MissionList.Add(new Proto.Mission
        {
            Id = (uint)missionId,
            Progress = (uint)(currentProgress + progress)
        });

        await Player.SendPacket(new PacketPlayerSyncScNotify(sync));
    }

    public async ValueTask SetMissionProgress(int missionId, int progress)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return;

        Data.SubMissionProgressDict[missionId] = progress;
        GameData.SubMissionData.TryGetValue(missionId, out var subMission);
        if (subMission == null) return;

        if (progress >= (subMission.SubMissionInfo?.Progress ?? 1)) return;

        var sync = new MissionSync();
        sync.MissionList.Add(new Proto.Mission
        {
            Id = (uint)missionId,
            Progress = (uint)progress
        });

        await Player.SendPacket(new PacketPlayerSyncScNotify(sync));
    }

    #endregion

    #region Mission Status

    public MissionPhaseEnum GetMainMissionStatus(int missionId)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return MissionPhaseEnum.Finish;

        return Data.GetMainMissionStatus(missionId);
    }

    public MissionPhaseEnum GetSubMissionStatus(int missionId)
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return MissionPhaseEnum.Finish;

        return Data.GetSubMissionStatus(missionId);
    }

    public SubMissionInfo? GetSubMissionInfo(int missionId)
    {
        GameData.SubMissionData.TryGetValue(missionId, out var subMission);
        if (subMission == null) return null;
        return subMission.SubMissionInfo;
    }

    public List<int> GetRunningSubMissionIdList()
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return [];

        var list = new List<int>();
        list.AddRange(Data.RunningSubMissionIds);
        return list;
    }

    public List<SubMissionInfo> GetRunningSubMissionList()
    {
        if (!ConfigManager.Config.ServerOption.EnableMission) return [];

        var list = new List<SubMissionInfo>();
        var ids = new List<int>();
        ids.AddRange(Data.RunningSubMissionIds);
        foreach (var id in ids)
        {
            GameData.SubMissionData.TryGetValue(id, out var mission);
            if (mission != null && mission.SubMissionInfo != null) list.Add(mission.SubMissionInfo);
        }

        return list;
    }

    public int GetMissionProgress(int missionId)
    {
        GameData.SubMissionData.TryGetValue(missionId, out var subMission);
        if (!ConfigManager.Config.ServerOption.EnableMission) return subMission?.SubMissionInfo?.Progress ?? 0;

        Data.SubMissionProgressDict.TryGetValue(missionId, out var progress);
        return progress;
    }

    #endregion

    #region Handlers

    public async ValueTask OnBattleFinish(PVEBattleResultCsReq req, BattleInstance instance)
    {
        foreach (var mission in GetRunningSubMissionIdList())
        {
            var subMission = GetSubMissionInfo(mission);
            if (subMission != null && subMission.FinishType == MissionFinishTypeEnum.StageWin &&
                req.EndStatus == BattleEndStatus.BattleEndWin) // TODO: Move to handler
                if (req.StageId.ToString().StartsWith(subMission.ParamInt1.ToString()))
                {
                    await FinishSubMission(mission);
                    instance.EventId = 0;
                }
        }

        await HandleAllFinishType(instance);
    }

    public async ValueTask OnPlayerInteractWithProp()
    {
        foreach (var id in GetRunningSubMissionIdList())
            if (GetSubMissionInfo(id)?.FinishType == MissionFinishTypeEnum.PropState)
            {
                FinishTypeHandlers.TryGetValue(MissionFinishTypeEnum.PropState, out var handler);
                if (handler != null) await handler.HandleMissionFinishType(Player, GetSubMissionInfo(id)!, null);
            }
    }

    public async ValueTask OnPlayerChangeScene()
    {
        foreach (var id in GetRunningSubMissionIdList())
        {
            var info = GetSubMissionInfo(id);
            if (info == null) continue;

            if (info.LevelFloorID == Player.SceneInstance?.FloorId)
            {
                if (info.GroupIDList == null) continue;
                foreach (var group in info.GroupIDList)
                    await Player.SceneInstance.EntityLoader!.LoadGroup(group, false);
            }
        }
    }

    public void OnLoadScene(SceneInfo info)
    {
        foreach (var mainMission in GameData.MainMissionData.Values)
        {
            foreach (var subMission in mainMission.MissionInfo?.SubMissionList ?? [])
                if (subMission.LevelFloorID == info.FloorId)
                    info.SceneMissionInfo.SubMissionStatusList.Add(new Proto.Mission
                    {
                        Id = (uint)subMission.ID,
                        Status = GetSubMissionStatus(subMission.ID).ToProto(),
                        Progress = (uint)GetMissionProgress(subMission.ID)
                    });

            foreach (var subMission in mainMission.MissionInfo?.SubMissionList ?? [])
                if (subMission.LevelFloorID == info.FloorId)
                {
                    if (GetMainMissionStatus(mainMission.MainMissionID) == MissionPhaseEnum.Finish)
                        info.SceneMissionInfo.FinishedMainMissionIdList.Add((uint)mainMission.MainMissionID);
                    else if (GetMainMissionStatus(mainMission.MainMissionID) == MissionPhaseEnum.Accept)
                        info.SceneMissionInfo.UnfinishedMainMissionIdList.Add((uint)mainMission.MainMissionID);
                    break; // only one
                }
        }
    }

    #endregion
}