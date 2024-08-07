using EggLink.DanhengServer.Data.Config;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("PerformanceE.json")]
public class PerformanceEExcel : ExcelResource
{
    public int PerformanceID { get; set; }
    public string PerformancePath { get; set; } = "";

    [JsonIgnore] public LevelGraphConfigInfo? ActInfo { get; set; }

    public override int GetId()
    {
        return PerformanceID;
    }

    public override void Loaded()
    {
        GameData.PerformanceEData.Add(PerformanceID, this);
    }
}