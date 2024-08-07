using EggLink.DanhengServer.Data.Config.Task;

namespace EggLink.DanhengServer.Data.Config;

public class FetchAdvPropData
{
    public DynamicFloat GroupID { get; set; } = new();
    public DynamicFloat ID { get; set; } = new();
}