namespace EggLink.DanhengServer.Data.Config.Task;

public class CreateProp : TaskConfigInfo
{
    public DynamicFloat GroupID { get; set; } = new();
    public DynamicFloat GroupPropID { get; set; } = new();
    public List<GroupEntityInfo> CreateList { get; set; } = [];
}