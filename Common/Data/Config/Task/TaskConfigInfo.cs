using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.Task;

public class TaskConfigInfo
{
    public string Type { get; set; } = "";
    public bool TaskEnabled { get; set; } = false;

    public static TaskConfigInfo LoadFromJsonObject(JObject json)
    {
        var type = json[nameof(Type)]?.Value<string>() ?? "";
        if (string.IsNullOrEmpty(type)) return new TaskConfigInfo();

        var typeStr = type.Replace("RPG.GameCore.", "");
        var className = "EggLink.DanhengServer.Data.Config.Task." + typeStr;
        var typeClass = System.Type.GetType(className);
        if (typeStr == "PredicateTaskList")
        {
            var res = PredicateTaskList.LoadFromJsonObject(json);
            res.Type = type;
            return res;
        }

        if (typeStr == "PropSetupUITrigger")
        {
            var res = PropSetupUITrigger.LoadFromJsonObject(json);
            res.Type = type;
            return res;
        }

        if (typeStr == "PropStateExecute")
        {
            var res = PropStateExecute.LoadFromJsonObject(json);
            res.Type = type;
            return res;
        }

        if (typeStr == "AddMazeBuff")
        {
            var res = AddMazeBuff.LoadFromJsonObject(json);
            res.Type = type;
            return res;
        }

        if (typeStr == "RemoveMazeBuff")
        {
            var res = RemoveMazeBuff.LoadFromJsonObject(json);
            res.Type = type;
            return res;
        }

        if (typeStr == "RefreshMazeBuffTime")
        {
            var res = RefreshMazeBuffTime.LoadFromJsonObject(json);
            res.Type = type;
            return res;
        }

        if (typeClass != null)
        {
            var res = (TaskConfigInfo)json.ToObject(typeClass)!;
            res.Type = type;
            return res;
        }

        return JsonConvert.DeserializeObject<TaskConfigInfo>(json.ToString())!;
    }
}