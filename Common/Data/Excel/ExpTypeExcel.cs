using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("ExpType.json")]
    public class ExpTypeExcel : ExcelResource
    {
        public int TypeID { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }

        public override int GetId()
        {
            return (TypeID * 100) + Level;
        }

        public override void Loaded()
        {
            GameData.ExpTypeData.Add(GetId(), this);
        }
    }
}
