using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RogueTalent.json")]
    public class RogueTalentExcel : ExcelResource
    {
        public int TalentID { get; set; }
        public bool IsImportant { get; set; }

        public override int GetId()
        {
            return TalentID;
        }

        public override void Loaded()
        {
            GameData.RogueTalentData.Add(GetId(), this);
        }
    }
}
