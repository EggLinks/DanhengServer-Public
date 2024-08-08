using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Database.Quests;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Enums.Quest;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Quest;

public class QuestManager(PlayerInstance player) : BasePlayerManager(player)
{
    public UnlockHandler UnlockHandler { get; } = new(player);
    public QuestData QuestData { get; } = DatabaseHelper.Instance!.GetInstanceOrCreateNew<QuestData>(player.Uid);
    public List<QuestInfo> WaitToSync { get; } = [];

    #region Handler

    public void OnBattleStart(BattleInstance instance)
    {
        foreach (var questInfo in GetRunningQuest())
        {
            var questExcel = GameData.QuestDataData.GetValueOrDefault(questInfo.QuestId);
            if (questExcel == null) continue;
            var finishWayExcel = GameData.FinishWayData.GetValueOrDefault(questExcel.FinishWayID);
            if (finishWayExcel == null) continue;
            if (finishWayExcel.FinishType == MissionFinishTypeEnum.BattleChallenge)
                foreach (var target in finishWayExcel.ParamIntList)
                    instance.AddBattleTarget(2, target, GetQuestProgress(questExcel.QuestID), finishWayExcel.Progress);
        }
    }

    #endregion

    #region Actions

    public async ValueTask AcceptQuestByCondition()
    {
        var syncList = new List<QuestInfo>();
        foreach (var quest in GameData.QuestDataData.Values)
        {
            if (QuestData.Quests.ContainsKey(quest.QuestID)) continue; // Already accepted
            QuestInfo? acceptQuest = null;
            switch (quest.UnlockType)
            {
                case QuestUnlockTypeEnum.AutoUnlock:
                    acceptQuest = await AcceptQuest(quest.QuestID, false);
                    break;
                case QuestUnlockTypeEnum.FinishMission:
                    var accept = true;

                    foreach (var missionId in quest.UnlockParamList)
                        if (Player.MissionManager!.GetMainMissionStatus(missionId) != MissionPhaseEnum.Finish)
                        {
                            accept = false;
                            break;
                        }

                    if (accept) acceptQuest = await AcceptQuest(quest.QuestID, false);
                    break;
                case QuestUnlockTypeEnum.FinishQuest:
                    var accept2 = true;

                    foreach (var questId in quest.UnlockParamList)
                        if (GetQuestStatus(questId) != QuestStatus.QuestFinish &&
                            GetQuestStatus(questId) != QuestStatus.QuestClose)
                        {
                            accept2 = false;
                            break;
                        }

                    if (accept2) acceptQuest = await AcceptQuest(quest.QuestID, false);
                    break;
                case QuestUnlockTypeEnum.ManualUnlock: // idk what this is
                    break;
                case QuestUnlockTypeEnum.BattlePassWeekly:
                case QuestUnlockTypeEnum.Unknown:
                default:
                    break;
            }

            if (acceptQuest != null) syncList.Add(acceptQuest);
        }

        if (syncList.Count > 0)
            await Player.SendPacket(new PacketPlayerSyncScNotify(syncList));
    }

    public async ValueTask<QuestInfo?> AcceptQuest(int questId, bool sync = true)
    {
        GameData.QuestDataData.TryGetValue(questId, out var questExcel);
        if (questExcel == null) return null;

        if (QuestData.Quests.ContainsKey(questId)) return null;

        var questInfo = new QuestInfo
        {
            QuestId = questId,
            QuestStatus = QuestStatus.QuestDoing,
            Progress = 0
        };

        QuestData.Quests.Add(questId, questInfo);

        if (sync) await Player.SendPacket(new PacketPlayerSyncScNotify(questInfo));

        return questInfo;
    }

    public async ValueTask FinishQuest(int questId, bool push = true)
    {
        GameData.QuestDataData.TryGetValue(questId, out var questExcel);
        if (questExcel == null) return;
        GameData.FinishWayData.TryGetValue(questExcel.FinishWayID, out var finishWayExcel);
        if (finishWayExcel == null) return;

        if (!QuestData.Quests.TryGetValue(questId, out var questInfo)) return;
        if (questInfo.QuestStatus != QuestStatus.QuestDoing) return;

        questInfo.QuestStatus = QuestStatus.QuestFinish;
        questInfo.Progress = finishWayExcel.Progress;
        questInfo.FinishTime = DateTime.Now.ToUnixSec();
        if (push)
            await Player.SendPacket(new PacketPlayerSyncScNotify(questInfo));
        else
            WaitToSync.SafeAdd(questInfo);

        // accept next quest
        await AcceptQuestByCondition();
    }

    public async ValueTask<Retcode> FinishQuestByClient(int questId)
    {
        GameData.QuestDataData.TryGetValue(questId, out var questExcel);
        if (questExcel == null) return Retcode.RetFail;
        GameData.FinishWayData.TryGetValue(questExcel.FinishWayID, out var finishWayExcel);
        if (finishWayExcel == null) return Retcode.RetQuestStatusError;
        if (finishWayExcel.FinishType != MissionFinishTypeEnum.AutoFinish &&
            finishWayExcel.FinishType != MissionFinishTypeEnum.FinishQuestByClient) return Retcode.RetQuestStatusError;

        if (!QuestData.Quests.TryGetValue(questId, out var questInfo)) return Retcode.RetQuestNotAccept;
        if (questInfo.QuestStatus != QuestStatus.QuestDoing) return Retcode.RetQuestStatusError;

        questInfo.QuestStatus = QuestStatus.QuestFinish;
        questInfo.Progress = finishWayExcel.Progress;
        questInfo.FinishTime = DateTime.Now.ToUnixSec();
        await Player.SendPacket(new PacketPlayerSyncScNotify(questInfo));

        // accept next quest
        await AcceptQuestByCondition();

        return Retcode.RetSucc;
    }

    public async ValueTask<(Retcode, List<ItemData>?)> TakeQuestReward(int questId)
    {
        GameData.QuestDataData.TryGetValue(questId, out var questExcel);
        if (questExcel == null) return (Retcode.RetFail, null);

        if (!QuestData.Quests.TryGetValue(questId, out var questInfo)) return (Retcode.RetQuestNotAccept, null);
        if (questInfo.QuestStatus != QuestStatus.QuestFinish) return (Retcode.RetQuestNotFinish, null);

        questInfo.QuestStatus = QuestStatus.QuestClose; // Close the quest after taking the reward

        // handle reward
        var items = await Player.InventoryManager!.HandleReward(questExcel.RewardID);

        await Player.SendPacket(new PacketPlayerSyncScNotify(questInfo));

        return (Retcode.RetSucc, items);
    }

    public async ValueTask AddQuestProgress(int questId, int progress, bool push = false)
    {
        GameData.QuestDataData.TryGetValue(questId, out var questExcel);
        if (questExcel == null) return;
        GameData.FinishWayData.TryGetValue(questExcel.FinishWayID, out var finishWayExcel);
        if (finishWayExcel == null) return;

        if (!QuestData.Quests.TryGetValue(questId, out var questInfo)) return;
        if (questInfo.QuestStatus != QuestStatus.QuestDoing) return;

        questInfo.Progress += progress;
        if (questInfo.Progress >= finishWayExcel.Progress) await FinishQuest(questId, push);
        else if (push)
            await Player.SendPacket(new PacketPlayerSyncScNotify(questInfo));
        else
            WaitToSync.SafeAdd(questInfo);
    }

    public async ValueTask UpdateQuestProgress(int questId, int progress, bool push = false)
    {
        GameData.QuestDataData.TryGetValue(questId, out var questExcel);
        if (questExcel == null) return;
        GameData.FinishWayData.TryGetValue(questExcel.FinishWayID, out var finishWayExcel);
        if (finishWayExcel == null) return;

        if (!QuestData.Quests.TryGetValue(questId, out var questInfo)) return;
        if (questInfo.QuestStatus != QuestStatus.QuestDoing) return;

        if (progress < questInfo.Progress) return; // prevent rollback
        if (progress == questInfo.Progress) return;
        questInfo.Progress = progress;
        if (questInfo.Progress >= finishWayExcel.Progress) await FinishQuest(questId, push);
        else if (push)
            await Player.SendPacket(new PacketPlayerSyncScNotify(questInfo));
        else
            WaitToSync.SafeAdd(questInfo);
    }

    public async ValueTask SyncQuest()
    {
        if (WaitToSync.Count == 0) return;
        await Player.SendPacket(new PacketPlayerSyncScNotify(WaitToSync));
        WaitToSync.Clear();
    }

    #endregion

    #region Information

    public QuestStatus GetQuestStatus(int questId)
    {
        if (!QuestData.Quests.TryGetValue(questId, out var questInfo)) return QuestStatus.QuestNone;
        return questInfo.QuestStatus;
    }

    public List<QuestInfo> GetRunningQuest()
    {
        return QuestData.Quests.Values.Where(x => x.QuestStatus == QuestStatus.QuestDoing).ToList();
    }

    public int GetQuestProgress(int questId)
    {
        if (!QuestData.Quests.TryGetValue(questId, out var questInfo)) return 0;
        return questInfo.Progress;
    }

    #endregion
}