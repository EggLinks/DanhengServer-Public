using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("SummonUnitData.json")]
    public class SummonUnitExcel : ExcelResource
    {
        public int ID { get; set; } = 0;
        public string JsonPath { get; set; } = "";

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            GameData.SummonUnitData[ID] = this;
        }
    }
}
