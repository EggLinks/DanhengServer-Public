using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RogueMonster.json")]
    public class RogueMonsterExcel : ExcelResource
    {
        public int RogueMonsterID { get; set; }
        public int NpcMonsterID { get; set; }
        public int EventID { get; set; }

        public override int GetId()
        {
            return RogueMonsterID;
        }

        public override void Loaded()
        {
            GameData.RogueMonsterData.Add(RogueMonsterID, this);
        }
    }
}
