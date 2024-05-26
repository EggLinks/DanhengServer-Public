using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EggLink.DanhengServer.Data.Config
{
    public class MonsterInfo : PositionInfo
    {
        public int NPCMonsterID { get; set; }
        public int EventID { get; set; }
        public int FarmElementID { get; set; }
        public bool IsClientOnly { get; set; }
    }
}
