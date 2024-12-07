using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournBuffGroup.json")]
public class RogueTournBuffGroupExcel : BaseRogueBuffGroupExcel
{
    public int RogueBuffGroupID { get; set; }
    public List<int> RogueBuffDrop { get; set; } = [];

    public override int GetId()
    {
        return RogueBuffGroupID;
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
        foreach (var buffId in RogueBuffDrop)
        {
            List<RogueTournBuffExcel> buffs =
            [
                .. GameData.RogueBuffData.Where(x => x.Value is RogueTournBuffExcel).Select(x =>
                    (x.Value as RogueTournBuffExcel)!).ToList()
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
                if (group is not RogueTournBuffGroupExcel e) continue;
                e.LoadBuff();
                BuffList.SafeAddRange(e.BuffList);
                count++;
            }
        }

        if (count == RogueBuffDrop.Count) IsLoaded = true;
    }
}