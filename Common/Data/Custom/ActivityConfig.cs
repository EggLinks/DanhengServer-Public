using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Custom
{
    public class ActivityConfig
    {
        public List<ActivityScheduleData> ScheduleData { get; set; } = [];
    }

    public class ActivityScheduleData
    {
        public int ActivityId { get; set; }
        public long BeginTime { get; set; }
        public long EndTime { get; set; }
        public int PanelId { get; set; }
    }
}
