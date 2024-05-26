using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("QuestData.json")]
    public class QuestDataExcel : ExcelResource
    {
        public int QuestID { get; set; }
        public int QuestType { get; set; }
        public HashName QuestTitle { get; set; } = new();

        public override int GetId()
        {
            return QuestID;
        }

        public override void Loaded()
        {
            GameData.QuestDataData.Add(QuestID, this);
        }
    }
}
