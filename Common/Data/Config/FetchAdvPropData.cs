using EggLink.DanhengServer.Data.Config.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Config
{
    public class FetchAdvPropData
    {
        public DynamicFloat GroupID { get; set; } = new();
        public DynamicFloat ID { get; set; } = new();
    }
}
