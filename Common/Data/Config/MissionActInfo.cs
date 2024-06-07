using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config
{
    public class MissionActInfo
    {
        public List<MissionActTaskInfo> OnInitSequece { get; set; } = [];
        public List<MissionActTaskInfo> OnStartSequece { get; set; } = [];
    }

    public class MissionActTaskInfo
    {
        public List<MissionActTaskInfo> TaskList { get; set; } = [];
        public string Type { get; set; } = "";
        public int MessageSectionID { get; set; }
    }
}
