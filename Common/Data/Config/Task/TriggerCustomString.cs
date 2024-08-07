namespace EggLink.DanhengServer.Data.Config.Task;

public class TriggerCustomString : TaskConfigInfo
{
    public DynamicString CustomString { get; set; } = new();
}

public class DynamicString
{
    public string Value { get; set; } = string.Empty;
}