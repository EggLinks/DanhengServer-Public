using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("EquipmentExpType.json")]
    public class EquipmentExpTypeExcel : ExcelResource
    {
        public int ExpType { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }

        public override int GetId()
        {
            return ExpType * 100 + Level;
        }

        public override void Loaded()
        {
            GameData.EquipmentExpTypeData.Add(GetId(), this);
        }
    }
}
