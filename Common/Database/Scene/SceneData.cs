using EggLink.DanhengServer.Enums.Scene;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Scene;

[SugarTable("Scene")]
public class SceneData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)]
    public Dictionary<int, Dictionary<int, List<ScenePropData>>> ScenePropData { get; set; } =
        []; // Dictionary<FloorId, Dictionary<GroupId, ScenePropData>>

    [SugarColumn(IsJson = true)]
    public Dictionary<int, List<int>> UnlockSectionIdList { get; set; } = []; // Dictionary<FloorId, List<SectionId>>

    [SugarColumn(IsJson = true)]
    public Dictionary<int, Dictionary<int, string>> CustomSaveData { get; set; } =
        []; // Dictionary<EntryId, Dictionary<GroupId, SaveData>>

    [SugarColumn(IsJson = true)]
    public Dictionary<int, Dictionary<string, int>> FloorSavedData { get; set; } =
        []; // Dictionary<FloorId, Dictionary<SaveDataKey, SaveDataValue>>
}

public class ScenePropData
{
    public int PropId;
    public PropStateEnum State;
}