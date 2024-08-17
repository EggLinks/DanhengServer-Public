using EggLink.DanhengServer.Util;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueBuffGroup.json")]
public class RogueBuffGroupExcel : ExcelResource
{
    [JsonProperty("MNNPAFJEGJC")] public int GroupID { get; set; }

    [JsonProperty("KCFPNHGBGIA")] public List<int> BuffTagList { get; set; } = [];

    [JsonIgnore] public List<RogueBuffExcel> BuffList { get; set; } = [];

    [JsonIgnore] public bool IsLoaded { get; set; }

    public override int GetId()
    {
        return GroupID;
    }

    public override void Loaded()
    {
        GameData.RogueBuffGroupData.Add(GetId(), this);
        LoadBuff();
    }

    public override void AfterAllDone()
    {
        LoadBuff();
    }

    public void LoadBuff()
    {
        if (IsLoaded) return;
        var count = 0;
        foreach (var buffId in BuffTagList)
            if (GameData.RogueBuffData.FirstOrDefault(x => x.Value.RogueBuffTag == buffId).Value is RogueBuffExcel buff)
            {
                BuffList.SafeAdd(buff);
                count++;
            }
            else
            {
                // might is group id
                if (!GameData.RogueBuffGroupData.TryGetValue(buffId, out var group)) continue;
                group.LoadBuff();
                BuffList.SafeAddRange(group.BuffList);
                count++;
            }

        if (count == BuffTagList.Count) IsLoaded = true;
    }
}