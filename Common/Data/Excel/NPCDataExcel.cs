using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("NPCData.json")]
    public class NPCDataExcel : ExcelResource
    {
        public int ID { get; set; }

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            GameData.NpcDataData.Add(ID, this);
        }
    }
}
