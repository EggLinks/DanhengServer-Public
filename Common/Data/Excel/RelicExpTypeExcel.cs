using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RelicExpType.json")]
    public class RelicExpTypeExcel : ExcelResource
    {
        public int TypeID { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }

        public override int GetId()
        {
            return TypeID * 100 + Level;
        }

        public override void Loaded()
        {
            GameData.RelicExpTypeData.Add(GetId(), this);
        }
    }
}
