namespace EggLink.DanhengServer.Data.Config.Task;

public class DestroyProp : TaskConfigInfo
{
    public DynamicFloat ID { get; set; } = new();
    public DynamicFloat GroupID { get; set; } = new();
    public List<GroupEntityInfo> DestroyList { get; set; } = [];
}

public class GroupEntityInfo
{
    public DynamicFloat GroupID { get; set; } = new();
    public DynamicFloat GroupInstanceID { get; set; } = new();
}

public class DynamicFloat
{
    public bool IsDynamic { get; set; }
    public FixedValueInfo<int> FixedValue { get; set; } = new();

    public int GetValue()
    {
        return IsDynamic ? 0 : FixedValue.Value;
    }
}