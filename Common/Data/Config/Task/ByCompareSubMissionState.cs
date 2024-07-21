using EggLink.DanhengServer.Enums.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config.Task
{
    public class ByCompareSubMissionState : PredicateConfigInfo
    {
        public int SubMissionID { get; set; }
        public SubMissionStateEnum SubMissionState { get; set; }
        public bool AllStoryLine { get; set; }
    }
}
