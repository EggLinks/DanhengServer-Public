using EggLink.DanhengServer.Enums.Task;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config.Task
{
    public class ByCompareFloorSavedValue : PredicateConfigInfo
    {
        public string Name { get; set; } = "";

        [JsonConverter(typeof(StringEnumConverter))]
        public CompareTypeEnum CompareType { get; set; } = CompareTypeEnum.Equal;
        public short CompareValue { get; set; } = 0;
    }
}
