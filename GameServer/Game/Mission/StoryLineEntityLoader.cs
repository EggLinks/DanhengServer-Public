using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.GameServer.Game.Mission;

public class StoryLineEntityLoader(SceneInstance scene) : SceneEntityLoader(scene)
{
    public int DimensionId;

    public override async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;

        var storyId = Scene.Player.StoryLineManager?.StoryLineData.CurStoryLineId ?? 0;
        if (storyId == 0) return;

        GameData.StoryLineFloorDataData.TryGetValue(storyId, out var floorData);
        if (floorData == null) return;

        floorData.TryGetValue(Scene.FloorInfo?.FloorID ?? 0, out var floorInfo);
        floorInfo ??= new StoryLineFloorDataExcel { DimensionID = 0 }; // Default

        var dim = Scene.FloorInfo?.DimensionList.Find(d => d.ID == floorInfo.DimensionID);
        if (dim == null) return;

        DimensionId = dim.ID;

        LoadGroups.AddRange(dim.GroupIDList);

        foreach (var group in Scene.FloorInfo?.Groups.Values!) // Sanity check in SceneInstance
        {
            if (group.LoadSide == GroupLoadSideEnum.Client) continue;
            if (group.GroupName.Contains("TrainVisitor")) continue;
            await LoadGroup(group);
        }

        Scene.IsLoaded = true;
    }

    public override async ValueTask<List<IGameEntity>?> LoadGroup(GroupInfo info, bool forceLoad = false)
    {
        if (!LoadGroups.Contains(info.Id)) return null;
        return await base.LoadGroup(info, forceLoad);
    }
}