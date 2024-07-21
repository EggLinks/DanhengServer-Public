using EggLink.DanhengServer.Data.Config.Task;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config
{
    public class LevelStartSequeceConfigInfo
    {
        public List<TaskConfigInfo> TaskList { get; set; } = [];
        public bool IsLoop { get; set; } = false;
        public int Order { get; set; }

        public static LevelStartSequeceConfigInfo LoadFromJsonObject(JObject obj)
        {
            LevelStartSequeceConfigInfo info = new();
            if (obj.ContainsKey(nameof(TaskList)))
            {
                info.TaskList = obj[nameof(TaskList)]?.Select(x => TaskConfigInfo.LoadFromJsonObject((x as JObject)!)).ToList() ?? [];
            }

            if (obj.ContainsKey(nameof(IsLoop)))
            {
                info.IsLoop = obj[nameof(IsLoop)]?.Value<bool>() ?? false;
            }

            if (obj.ContainsKey(nameof(Order)))
            {
                info.Order = obj[nameof(Order)]?.Value<int>() ?? 0;
            }
            return info;
        }
    }
}
