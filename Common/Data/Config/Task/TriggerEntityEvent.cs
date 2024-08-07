namespace EggLink.DanhengServer.Data.Config.Task;

public class TriggerEntityEvent : TaskConfigInfo
{
    public DynamicFloat InstanceID { get; set; } = new();
}