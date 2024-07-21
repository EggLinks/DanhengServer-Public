using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config.Task
{
    public class PredicateConfigInfo : TaskConfigInfo
    {
        public bool Inverse { get; set; } = false;

        public static new PredicateConfigInfo LoadFromJsonObject(JObject obj)
        {
            PredicateConfigInfo info = new();
            info.Type = obj[nameof(Type)]!.ToObject<string>()!;

            var typeStr = info.Type.Replace("RPG.GameCore.", "");
            var className = "EggLink.DanhengServer.Data.Config.Task." + typeStr;
            var typeClass = System.Type.GetType(className);
            if (typeClass != null)
            {
                info = (PredicateConfigInfo)obj.ToObject(typeClass)!;
            }
            else
            {
                info = Newtonsoft.Json.JsonConvert.DeserializeObject<PredicateConfigInfo>(obj.ToString())!;
            }
            return info;
        }
    }
}
