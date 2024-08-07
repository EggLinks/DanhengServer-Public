using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Config;

public class DialogueInfo
{
    public List<DialogueTaskInfo> OnInitSequece { get; set; } = [];
    public List<DialogueTaskInfo> OnStartSequece { get; set; } = [];

    [JsonIgnore] public List<int> DialogueIds { get; set; } = [];

    public void Loaded()
    {
        foreach (var task in OnInitSequece)
        foreach (var ta in task.TaskList)
        foreach (var option in ta.OptionList)
            DialogueIds.Add(option.DialogueEventID);

        foreach (var task in OnStartSequece)
        foreach (var ta in task.TaskList)
        foreach (var option in ta.OptionList)
            DialogueIds.Add(option.DialogueEventID);
    }
}

public class DialogueTaskInfo
{
    public List<DialogueTaskInfo> TaskList { get; set; } = [];
    public List<DialogueTaskInfo> OptionList { get; set; } = [];

    public int DialogueEventID { get; set; }
}