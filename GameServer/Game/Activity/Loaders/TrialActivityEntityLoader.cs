using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Activity.Loaders;

public class TrialActivityEntityLoader(SceneInstance scene, PlayerInstance player) : SceneEntityLoader(scene)
{
    public PlayerInstance Player = player;

    public override async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;

        // Get activity instance
        if (Player.ActivityManager!.TrialActivityInstance == null) return;
        var instance = Player.ActivityManager!.TrialActivityInstance;
        LoadGroups.SafeAddRange(Scene.FloorInfo!.Groups.Keys.ToList());

        // Setup stage
        GameData.AvatarDemoConfigData.TryGetValue(instance.Data.CurTrialStageId, out var excel);
        if (excel == null) return;
        Scene.FloorInfo!.Groups.TryGetValue(excel.MazeGroupID1, out var groupData);
        if (groupData != null) await LoadGroup(groupData);

        foreach (var group in Scene.FloorInfo.Groups.Values)
        {
            // Skip non-server groups
            if (group.LoadSide != GroupLoadSideEnum.Server) continue;

            await LoadGroup(group);
        }

        Scene.IsLoaded = true;
    }

    public override async ValueTask<EntityMonster?> LoadMonster(MonsterInfo info, GroupInfo group,
        bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;

        // Get challenge instance
        if (Player.ActivityManager!.TrialActivityInstance == null) return null;
        var instance = Player.ActivityManager!.TrialActivityInstance;

        // Get current stage monster infos
        GameData.AvatarDemoConfigData.TryGetValue(instance.Data.CurTrialStageId, out var excel);
        if (excel == null) return null;
        Dictionary<int, AvatarDemoConfigExcel.StageMonsterInfo> stageMonsters;
        if (excel.MazeGroupID1 == group.Id)
            stageMonsters = excel.StageMonsters1;
        else
            return null;

        // Get challenge monster info
        if (!stageMonsters.ContainsKey(info.ID)) return null;
        var stageMonsterInfo = stageMonsters[info.ID];

        // Get excels from game data
        if (!GameData.NpcMonsterDataData.ContainsKey(stageMonsterInfo.NpcMonsterId)) return null;
        var npcMonsterExcel = GameData.NpcMonsterDataData[stageMonsterInfo.NpcMonsterId];

        // Create monster from group monster info
        var entity = new EntityMonster(Scene, info.ToPositionProto(), info.ToRotationProto(), group.Id, info.ID,
            npcMonsterExcel, info);
        entity.EventID = stageMonsterInfo.EventId;
        entity.CustomStageID = stageMonsterInfo.EventId;
        await Scene.AddEntity(entity, sendPacket);

        return entity;
    }

    public override async ValueTask<EntityNpc?> LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
    {
        await System.Threading.Tasks.Task.CompletedTask;
        return null;
    }
}