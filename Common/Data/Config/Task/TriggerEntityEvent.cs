using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config.Task
{
    public class TriggerEntityEvent : TaskConfigInfo
    { 
        public DynamicFloat InstanceID { get; set; } = new();
    }
}
