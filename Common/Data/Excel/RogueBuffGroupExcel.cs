using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueBuffGroup.json")]
public class RogueBuffGroupExcel : BaseRogueBuffGroupExcel
{
    [JsonProperty("HFLJEIPCCNF")] public int GroupID { get; set; }

    [JsonProperty("ILLJGPJPFAC")] public List<int> BuffTagList { get; set; } = [];

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
        {
            List<RogueBuffExcel> buffs =
            [
                .. GameData.RogueBuffData.Where(x => x.Value is RogueBuffExcel).Select(x =>
                    (x.Value as RogueBuffExcel)!).ToList()
            ];
            if (buffs.FirstOrDefault(x => x.RogueBuffTag == buffId) is { } buff)
            {
                BuffList.SafeAdd(buff);
                count++;
            }
            else
            {
                // might is group id
                if (!GameData.RogueBuffGroupData.TryGetValue(buffId, out var group)) continue;
                if (group is not RogueBuffGroupExcel e) continue;
                e.LoadBuff();
                BuffList.SafeAddRange(e.BuffList);
                count++;
            }
        }

        if (count == BuffTagList.Count) IsLoaded = true;
    }
}