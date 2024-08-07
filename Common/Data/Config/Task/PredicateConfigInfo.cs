using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.Task;

public class PredicateConfigInfo : TaskConfigInfo
{
    public bool Inverse { get; set; } = false;

    public new static PredicateConfigInfo LoadFromJsonObject(JObject obj)
    {
        PredicateConfigInfo info = new();
        info.Type = obj[nameof(Type)]!.ToObject<string>()!;

        var typeStr = info.Type.Replace("RPG.GameCore.", "");
        var className = "EggLink.DanhengServer.Data.Config.Task." + typeStr;
        var typeClass = System.Type.GetType(className);
        if (typeClass != null)
            info = (PredicateConfigInfo)obj.ToObject(typeClass)!;
        else
            info = JsonConvert.DeserializeObject<PredicateConfigInfo>(obj.ToString())!;
        return info;
    }
}