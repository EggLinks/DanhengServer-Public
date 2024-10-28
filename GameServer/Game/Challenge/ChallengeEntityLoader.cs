using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Challenge;

public class ChallengeEntityLoader(SceneInstance scene, PlayerInstance player) : SceneEntityLoader(scene)
{
    public PlayerInstance Player = player;

    public override async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;

        // Get challenge instance
        if (Player.ChallengeManager!.ChallengeInstance == null) return;
        var instance = Player.ChallengeManager.ChallengeInstance;
        LoadGroups.SafeAddRange(Scene.FloorInfo!.Groups.Keys.ToList());

        // Setup first stage
        var excel = instance.Excel;
        Scene.FloorInfo.Groups.TryGetValue(excel.MazeGroupID1, out var groupData);
        if (groupData != null) await LoadGroup(groupData);

        // Set leave entry
        Scene.LeaveEntryId =
            instance.IsStory() ? GameConstants.CHALLENGE_STORY_ENTRANCE : GameConstants.CHALLENGE_ENTRANCE;

        if (instance.IsBoss())
            Scene.LeaveEntryId = GameConstants.CHALLENGE_BOSS_ENTRANCE;

        foreach (var group in Scene.FloorInfo.Groups.Values)
        {
            // Skip non-server groups
            if (group.LoadSide != GroupLoadSideEnum.Server) continue;

            // Dont load the groups that have monsters in them
            if (group.PropList.Count > 0 && group.MonsterList.Count > 0) await LoadGroup(group);
        }

        Scene.IsLoaded = true;
    }

    public override async ValueTask<EntityMonster?> LoadMonster(MonsterInfo info, GroupInfo group,
        bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;

        // Get challenge instance
        if (Player.ChallengeManager!.ChallengeInstance == null) return null;
        var instance = Player.ChallengeManager.ChallengeInstance;

        // Get current stage monster infos
        Dictionary<int, ChallengeConfigExcel.ChallengeMonsterInfo> challengeMonsters;
        if (instance.Excel.MazeGroupID1 == group.Id || instance.Excel.MazeGroupID2 == group.Id)
            challengeMonsters = instance.CurrentStage == 1
                ? instance.Excel.ChallengeMonsters1
                : instance.Excel.ChallengeMonsters2;
        else
            return null;

        // Get challenge monster info
        if (!challengeMonsters.ContainsKey(info.ID)) return null;
        var challengeMonsterInfo = challengeMonsters[info.ID];

        // Get excels from game data
        if (!GameData.NpcMonsterDataData.ContainsKey(challengeMonsterInfo.NpcMonsterId)) return null;
        var npcMonsterExcel = GameData.NpcMonsterDataData[challengeMonsterInfo.NpcMonsterId];

        // Create monster from group monster info
        var entity = new EntityMonster(Scene, info.ToPositionProto(), info.ToRotationProto(), group.Id, info.ID,
            npcMonsterExcel, info)
        {
            EventID = challengeMonsterInfo.EventId,
            CustomStageID = challengeMonsterInfo.EventId
        };
        await Scene.AddEntity(entity, sendPacket);

        return entity;
    }

    public override async ValueTask<EntityNpc?> LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
    {
        await System.Threading.Tasks.Task.CompletedTask;
        return null;
    }
}