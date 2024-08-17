using EggLink.DanhengServer.Util;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournBuffGroup.json")]
public class RogueTournBuffGroupExcel : ExcelResource
{
    public int RogueBuffGroupID { get; set; }
    public List<int> RogueBuffDrop { get; set; } = [];

    [JsonIgnore] public List<RogueTournBuffExcel> BuffList { get; set; } = [];
    [JsonIgnore] public bool IsLoaded { get; set; }


    public override int GetId()
    {
        return RogueBuffGroupID;
    }

    public override void Loaded()
    {
        GameData.RogueTournBuffGroupData.Add(GetId(), this);
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
        foreach (var buffId in RogueBuffDrop)
            if (GameData.RogueTournBuffData.FirstOrDefault(x => x.Value.RogueBuffTag == buffId).Value is { } buff)
            {
                BuffList.SafeAdd(buff);
                count++;
            }
            else
            {
                // might is group id
                if (!GameData.RogueTournBuffGroupData.TryGetValue(buffId, out var group)) continue;
                group.LoadBuff();
                BuffList.SafeAddRange(group.BuffList);
                count++;
            }

        if (count == RogueBuffDrop.Count) IsLoaded = true;
    }
}