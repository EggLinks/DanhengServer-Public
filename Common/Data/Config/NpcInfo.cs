using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EggLink.DanhengServer.Data.Config
{
    public class NpcInfo : PositionInfo
    {
        public int NPCID { get; set; }
        public bool IsClientOnly { get; set; }
    }
}
