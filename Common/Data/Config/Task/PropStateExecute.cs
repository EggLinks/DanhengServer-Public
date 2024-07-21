using EggLink.DanhengServer.Enums.Scene;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config.Task
{
    public class PropStateExecute : TaskConfigInfo
    {
        public TargetEvaluator TargetType { get; set; } = new();
        [JsonConverter(typeof(StringEnumConverter))]
        public PropStateEnum State { get; set; } = PropStateEnum.Closed;
        public List<TaskConfigInfo> Execute { get; set; } = [];

        public static new TaskConfigInfo LoadFromJsonObject(JObject obj)
        {
            var info = new PropStateExecute();

            info.Type = obj[nameof(Type)]!.ToObject<string>()!;
            if (obj.ContainsKey(nameof(TargetType)))
            {
                var targetType = obj[nameof(TargetType)] as JObject;
                var classType = System.Type.GetType($"EggLink.DanhengServer.Data.Config.Task.{targetType?["Type"]?.ToString().Replace("RPG.GameCore.", "")}");
                classType ??= System.Type.GetType($"EggLink.DanhengServer.Data.Config.Task.TargetEvaluator");
                info.TargetType = (targetType!.ToObject(classType!) as TargetEvaluator)!;
            }

            if (obj.ContainsKey(nameof(State)))
            {
                info.State = obj[nameof(State)]?.ToObject<PropStateEnum>() ?? PropStateEnum.Closed;
            }

            foreach (var item in obj[nameof(Execute)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)) ?? [])
            {
                info.Execute.Add(item);
            }

            return info;
        }
    }
}
