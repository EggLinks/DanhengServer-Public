using EggLink.DanhengServer.Enums.Task;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Config.Task;

public class TriggerPerformance : TaskConfigInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ELevelPerformanceTypeEnum PerformanceType { get; set; }

    public int PerformanceID { get; set; }
}