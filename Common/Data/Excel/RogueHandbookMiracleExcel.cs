using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RogueHandbookMiracle.json")]
    public class RogueHandbookMiracleExcel : ExcelResource
    {
        public int MiracleHandbookID { get; set; }
        public int MiracleReward { get; set; }

        public List<int> MiracleTypeList { get; set; } = [];

        public override int GetId()
        {
            return MiracleHandbookID;
        }

        public override void Loaded()
        {
            GameData.RogueHandbookMiracleData.Add(GetId(), this);
        }
    }
}
