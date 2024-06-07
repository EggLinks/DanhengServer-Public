using EggLink.DanhengServer.Data.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("PerformanceE.json")]
    public class PerformanceEExcel : ExcelResource
    {
        public int PerformanceID { get; set; }
        public string PerformancePath { get; set; } = "";

        [JsonIgnore]
        public MissionActInfo? ActInfo { get; set; }

        public override int GetId()
        {
            return PerformanceID;
        }

        public override void Loaded()
        {
            GameData.PerformanceEData.Add(PerformanceID, this);
        }
    }
}
