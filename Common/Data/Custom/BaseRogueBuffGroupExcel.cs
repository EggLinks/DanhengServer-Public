using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Custom;

public abstract class BaseRogueBuffGroupExcel : ExcelResource
{
    [JsonIgnore] public List<BaseRogueBuffExcel> BuffList { get; set; } = [];
    [JsonIgnore] public bool IsLoaded { get; set; }
}