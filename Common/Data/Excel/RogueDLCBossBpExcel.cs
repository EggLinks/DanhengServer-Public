using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueDLCBossBp.json")]
public class RogueDLCBossBpExcel : ExcelResource
{
    public int BossBpID { get; set; }
    public List<BossAndFloorInfo> MonsterAndFloorList { get; set; } = [];
    public List<int> BossDecayList { get; set; } = [];

    public override int GetId()
    {
        return BossBpID;
    }

    public override void Loaded()
    {
        GameData.RogueDLCBossBpData.Add(BossBpID, this);
    }
}

public class BossAndFloorInfo
{
    [JsonProperty("LKPOGAKCEMO")] public int MonsterId { get; set; }
}